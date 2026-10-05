using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Excel;
using SchoolManagementSystem.Application.School.Students.Helpers;
using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

/// <summary>
/// Bulk student import from Excel.
/// <list type="number">
/// <item>Parse + validate every row (required fields, lists, class, dates, duplicate login emails).</item>
/// <item>If any row is invalid → nothing is saved, row-level errors are returned.</item>
/// <item>Otherwise each row is saved by sending the existing <see cref="InsertStudentInfoCommand"/>,
/// i.e. exactly the same business logic as <c>StudentInfoController.save-student</c>.</item>
/// </list>
/// Each row runs in its own DI scope (fresh DbContext/UnitOfWork) so an unexpected DB failure
/// on one row cannot leave tracked entities behind and break the following rows.
/// </summary>
public class ImportStudentExcelCommandHandler : IHttpRequestHandler<ImportStudentExcelCommand>
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceScopeFactory _scopeFactory;

    public ImportStudentExcelCommandHandler(IUnitOfWork unitOfWork, IServiceScopeFactory scopeFactory)
    {
        _unitOfWork = unitOfWork;
        _scopeFactory = scopeFactory;
    }

    public async Task<IResult> Handle(ImportStudentExcelCommand request, CancellationToken cancellationToken)
    {
        // ---- 1. File checks --------------------------------------------------------------
        var file = request?.File;
        if (file == null || file.Length == 0)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "Please select an Excel file to upload.");
        }
        if (!string.Equals(Path.GetExtension(file.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "Only .xlsx files are supported. Please use the sample template.");
        }
        if (file.Length > MaxFileSizeBytes)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "The file is too large. Maximum allowed size is 10 MB.");
        }

        // ---- 2. Parse ------------------------------------------------------------------
        // Same source as the admission form's "Application For Class" dropdown.
        var classes = await _unitOfWork.AcademicClassRepository.GetAllNoneDeleted(true, true)
            .Where(x => x.IsActive)
            .Select(x => new { x.Id, x.ClassName })
            .ToListAsync(cancellationToken);
        var classLookup = classes
            .Where(x => !string.IsNullOrWhiteSpace(x.ClassName))
            .GroupBy(x => x.ClassName.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);

        StudentExcelParseResult parsed;
        await using (var stream = file.OpenReadStream())
        {
            parsed = StudentExcelParser.Parse(stream, classLookup);
        }

        if (parsed.FileError != null)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, parsed.FileError);
        }
        if (parsed.Rows.Count == 0)
        {
            return Result.Fail(StatusCodes.Status400BadRequest, "No student rows were found in the file.");
        }

        // ---- 3. Duplicate login-user check --------------------------------------------
        // InsertStudentInfoCommandHandler creates a User whose Email is derived from FullName,
        // and User.Email is globally unique. Detect collisions up-front (within the file and
        // against existing users) so the whole file is rejected before anything is saved.
        await ValidateLoginEmailsAsync(parsed.Rows, cancellationToken);

        var response = new StudentExcelImportResponse { TotalRows = parsed.Rows.Count };

        var invalidRows = parsed.Rows.Where(r => !r.IsValid).ToList();
        if (invalidRows.Count > 0)
        {
            response.ValidationFailed = true;
            response.FailedCount = invalidRows.Count;
            response.Errors = invalidRows.Select(ToRowError).ToList();
            return new Result<StudentExcelImportResponse>(
                StatusCodes.Status400BadRequest,
                response,
                Array.Empty<string>(),
                $"{invalidRows.Count} of {parsed.Rows.Count} row(s) have errors. No students were saved. Please fix the errors and upload again.");
        }

        // ---- 4. Save each row through the existing insert command ------------------------
        foreach (var row in parsed.Rows)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                var result = await sender.Send(new InsertStudentInfoCommand { StudentInfo = row.Request }, cancellationToken);

                if (result.IsSuccess)
                {
                    var data = (result as IResult<StudentInfoResponse>)?.Data;
                    response.Imported.Add(new StudentExcelImportedRow
                    {
                        RowNumber = row.RowNumber,
                        StudentId = data?.Id ?? Guid.Empty,
                        StdCID = data?.StdCID,
                        FullName = row.Request.FullName,
                    });
                }
                else
                {
                    var messages = result.Errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToList() ?? new List<string>();
                    if (!string.IsNullOrWhiteSpace(result.NotificationMessage)) messages.Add(result.NotificationMessage);
                    if (messages.Count == 0) messages.Add($"Save failed (status {result.StatusCode}).");
                    response.Errors.Add(new StudentExcelRowError { RowNumber = row.RowNumber, FullName = row.Request.FullName, Messages = messages });
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(new StudentExcelRowError
                {
                    RowNumber = row.RowNumber,
                    FullName = row.Request.FullName,
                    Messages = new List<string> { FriendlyError(ex) },
                });
            }
        }

        response.ImportedCount = response.Imported.Count;
        response.FailedCount = response.Errors.Count;

        var message = response.FailedCount == 0
            ? $"{response.ImportedCount} student(s) imported successfully."
            : $"{response.ImportedCount} student(s) imported, {response.FailedCount} failed.";

        // 200 when at least one row was saved (the client must refresh the list either way).
        var status = response.ImportedCount > 0 ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
        return new Result<StudentExcelImportResponse>(status, response, Array.Empty<string>(), message);
    }

    // -------------------------------------------------------------------------------------
    private async Task ValidateLoginEmailsAsync(List<StudentExcelParsedRow> rows, CancellationToken cancellationToken)
    {
        var rowsWithName = rows
            .Where(r => !string.IsNullOrWhiteSpace(r.Request.FullName))
            .Select(r => new { Row = r, Email = StudentUserHelper.BuildLoginEmail(r.Request.FullName!) })
            .ToList();

        if (rowsWithName.Count == 0) return;

        // Duplicates inside the uploaded file.
        foreach (var group in rowsWithName.GroupBy(x => x.Email, StringComparer.OrdinalIgnoreCase).Where(g => g.Count() > 1))
        {
            var rowNumbers = string.Join(", ", group.Select(x => x.Row.RowNumber));
            foreach (var item in group)
            {
                item.Row.Errors.Add($"Full Name '{item.Row.Request.FullName}' is duplicated in the file (rows {rowNumbers}). Each student's name must be unique because it is used for the login user.");
            }
        }

        // Existing users (unique index on User.Email is global, so ignore the tenant guard).
        var emails = rowsWithName.Select(x => x.Email).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var chunk in emails.Chunk(500))
        {
            var found = await _unitOfWork.UserRepository.GetAllNoneDeleted(true, true)
                .Where(u => chunk.Contains(u.Email))
                .Select(u => u.Email)
                .ToListAsync(cancellationToken);
            existing.UnionWith(found);
        }

        foreach (var item in rowsWithName.Where(x => existing.Contains(x.Email)))
        {
            item.Row.Errors.Add($"A user for '{item.Row.Request.FullName}' already exists ({item.Email}). A student with the same name is already registered.");
        }
    }

    private static StudentExcelRowError ToRowError(StudentExcelParsedRow row) => new()
    {
        RowNumber = row.RowNumber,
        FullName = row.Request.FullName,
        Messages = row.Errors.ToList(),
    };

    private static string FriendlyError(Exception ex)
    {
        var inner = ex;
        while (inner.InnerException != null) inner = inner.InnerException;

        var message = inner.Message;
        if (message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) && message.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            return "A user with the same name already exists (duplicate login email).";
        }
        return $"Save failed: {message}";
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AdmitCards.Helpers;
using SchoolManagementSystem.Application.School.AdmitCards.Models;
using SchoolManagementSystem.Application.School.AdmitCards.Queries;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.AdmitCards.Handlers.QueryHandlers;

public class DownloadAdmitCardQueryHandler : IHttpRequestHandler<DownloadAdmitCardQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public DownloadAdmitCardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(DownloadAdmitCardQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var studentId = request.StudentId;
            if (studentId == Guid.Empty)
            {
                return Result.Fail<AdmitCardPdfResponse>(StatusCodes.Status400BadRequest, "Student ID is required.");
            }

            var studentInfo = await _unitOfWork.StudentInfoRepository
                .GetAllNoneDeleted(false, true)
                .FirstOrDefaultAsync(x => x.Id == studentId, cancellationToken);

            var admission = await _unitOfWork.AdmissionRepository
                .GetAllNoneDeleted(false, true)
                .Include(x => x.Student)
                .Include(x => x.Class)
                .Include(x => x.Section)
                .Include(x => x.Shift)
                .Include(x => x.Branch)
                .Include(x => x.AcademicSession)
                .FirstOrDefaultAsync(x => x.StudentId == studentId || x.Id == studentId, cancellationToken);

            if (studentInfo == null && admission?.Student != null)
            {
                studentInfo = admission.Student;
                studentId = studentInfo.Id;
            }

            if (studentInfo == null && admission == null)
            {
                return Result.Fail<AdmitCardPdfResponse>(StatusCodes.Status404NotFound, "Student record not found.");
            }

            var sessionName = admission?.AcademicSession?.SessionName ?? studentInfo?.AcademicYear ?? "2026";
            var sessionTitle = sessionName.StartsWith("Session", StringComparison.OrdinalIgnoreCase) 
                ? sessionName 
                : $"Session {sessionName}";

            int sessionYear = DateTime.Now.Year;
            var digits = new string(sessionName.Where(char.IsDigit).Take(4).ToArray());
            if (!string.IsNullOrEmpty(digits) && int.TryParse(digits, out int parsedYear))
            {
                sessionYear = parsedYear;
            }

            var examType = (request.ExamType ?? "CT").ToUpperInvariant().Trim();
            string examTitle;
            string examDateStr;
            int cutoffMonth;
            int cutoffYear = sessionYear;

            if (examType.Contains("HALF") || examType.Contains("SEMESTER"))
            {
                examType = "HALF_YEARLY";
                examTitle = "Half Yearly Examination";
                examDateStr = $"15 Jun {sessionYear}";
                cutoffMonth = 6;
            }
            else if (examType.Contains("YEAR") || examType.Contains("ANNUAL") || examType.Contains("FINAL"))
            {
                examType = "ANNUAL";
                examTitle = "Yearly (Annual) Examination";
                examDateStr = $"15 Nov {sessionYear}";
                cutoffMonth = 11;
            }
            else
            {
                examType = "CT";
                examTitle = "Class Test (CT) Exam";
                examDateStr = $"10 Apr {sessionYear}";
                cutoffMonth = 4;
            }

            // Check Dues unless explicitly ignored (e.g. for preview)
            if (!request.IgnoreDueCheck)
            {
                var unpaidBills = await _unitOfWork.BillMasterRepository
                    .GetAllNoneDeleted(false, true)
                    .Where(x =>
                        !x.IsPaid &&
                        x.Admission != null &&
                        (x.Admission.StudentId == studentId || (admission != null && x.AdmissionId == admission.Id)) &&
                        (x.BillYear < cutoffYear || (x.BillYear == cutoffYear && x.BillMonth <= cutoffMonth))
                    )
                    .ToListAsync(cancellationToken);

                decimal dueAmount = 0;
                foreach (var b in unpaidBills)
                {
                    decimal due = b.DueAmount > 0 ? b.DueAmount : (b.TotalAmount - b.CollectionAmount);
                    if (due <= 0 && !b.IsPaid) due = b.TotalAmount;
                    dueAmount += due;
                }

                if (dueAmount > 0)
                {
                    return Result.Fail<AdmitCardPdfResponse>(
                        StatusCodes.Status400BadRequest,
                        $"Admit card cannot be downloaded. Student has an unpaid due amount of ৳ {dueAmount:N0} up to {cutoffMonth:D2}/{cutoffYear}."
                    );
                }
            }

            // Build PDF Model
            var model = new AdmitCardRenderModel
            {
                StudentName = studentInfo?.FullName ?? admission?.RollNo ?? "Student",
                ClassName = admission?.Class?.ClassName ?? studentInfo?.ApplicationForClass ?? "-",
                SectionName = admission?.Section?.SectionName ?? "-",
                RollNo = admission?.RollNo ?? studentInfo?.StdCID ?? "-",
                ExamStarts = examDateStr,
                Session = sessionTitle,
                ExamTitle = examTitle,
                ExamType = examType,
                SchoolName = "EdugAtes Integrated School",
                Tagline = "FAITH · WISDOM · REVIVAL · IMPACT",
                SchoolAddress = admission?.Branch?.BranchAddress ?? "[School address]",
                StudentImagePath = studentInfo?.ImagePath
            };

            var pdfBytes = AdmitCardPdfBuilder.GeneratePdf(model);

            var safeStudentName = string.Join("_", (model.StudentName ?? "Student").Split(Path.GetInvalidFileNameChars()));
            var fileName = $"AdmitCard_{examType}_{safeStudentName}_{sessionYear}.pdf";

            return Result.Success(new AdmitCardPdfResponse(pdfBytes, "application/pdf", fileName));
        }
        catch (Exception ex)
        {
            return Result.Fail<AdmitCardPdfResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

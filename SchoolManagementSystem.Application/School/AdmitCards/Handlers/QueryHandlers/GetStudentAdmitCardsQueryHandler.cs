using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AdmitCards.Models;
using SchoolManagementSystem.Application.School.AdmitCards.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.School.AdmitCards.Handlers.QueryHandlers;

public class GetStudentAdmitCardsQueryHandler : IHttpRequestHandler<GetStudentAdmitCardsQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentAdmitCardsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetStudentAdmitCardsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var studentId = request.StudentId;
            if (studentId == Guid.Empty)
            {
                return Result.Fail<StudentAdmitCardListResponse>(StatusCodes.Status400BadRequest, "Student ID is required.");
            }

            // Find student info (or via admission)
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
                return Result.Fail<StudentAdmitCardListResponse>(StatusCodes.Status404NotFound, "Student record not found.");
            }

            // Academic session info
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

            // Retrieve all unpaid bills for this student
            var unpaidBills = await _unitOfWork.BillMasterRepository
                .GetAllNoneDeleted(false, true)
                .Where(x =>
                    !x.IsPaid &&
                    x.Admission != null &&
                    (x.Admission.StudentId == studentId || (admission != null && x.AdmissionId == admission.Id))
                )
                .ToListAsync(cancellationToken);

            var now = DateTime.Now;

            // Template definitions for the 3 Admit Cards provided by user
            var examDefs = new[]
            {
                new
                {
                    Type = "CT",
                    Name = "Class Test (CT) Exam",
                    DateStr = $"10 Apr {sessionYear}",
                    Month = 4,
                    Year = sessionYear,
                    BadgeColor = "#0d603a"
                },
                new
                {
                    Type = "HALF_YEARLY",
                    Name = "Half Yearly Examination",
                    DateStr = $"15 Jun {sessionYear}",
                    Month = 6,
                    Year = sessionYear,
                    BadgeColor = "#233979"
                },
                new
                {
                    Type = "ANNUAL",
                    Name = "Yearly (Annual) Examination",
                    DateStr = $"15 Nov {sessionYear}",
                    Month = 11,
                    Year = sessionYear,
                    BadgeColor = "#9b1c2b"
                }
            };

            var admitCards = new List<StudentAdmitCardDto>();

            foreach (var def in examDefs)
            {
                // Dues until current date
                var currentBills = unpaidBills.Where(b =>
                    b.BillYear < now.Year ||
                    (b.BillYear == now.Year && b.BillMonth <= now.Month)
                );
                decimal dueCurrent = CalculateDueSum(currentBills);

                // Dues until exam date (cutoff month & year)
                var examBills = unpaidBills.Where(b =>
                    b.BillYear < def.Year ||
                    (b.BillYear == def.Year && b.BillMonth <= def.Month)
                );
                decimal dueExam = CalculateDueSum(examBills);

                // Check cutoff based on request or default (exam date vs current date)
                decimal effectiveDue = (request.CutoffType?.ToLower() == "current_date") ? dueCurrent : dueExam;

                bool isEligible = effectiveDue <= 0;

                admitCards.Add(new StudentAdmitCardDto
                {
                    ExamType = def.Type,
                    ExamName = def.Name,
                    Session = sessionTitle,
                    ExamStartDate = def.DateStr,
                    CutoffMonth = def.Month,
                    CutoffYear = def.Year,
                    DueAmount = effectiveDue,
                    DueAmountCurrentDate = dueCurrent,
                    DueAmountExamDate = dueExam,
                    IsEligible = isEligible,
                    Status = isEligible ? "Available" : $"Due: ৳ {effectiveDue:N0}",
                    BadgeColor = def.BadgeColor,
                    Reason = isEligible 
                        ? "Fees cleared. Ready to download." 
                        : $"Due of ৳ {effectiveDue:N0} must be paid before downloading."
                });
            }

            decimal totalOverallDue = CalculateDueSum(unpaidBills);

            var response = new StudentAdmitCardListResponse
            {
                StudentId = studentId,
                StudentName = studentInfo?.FullName ?? admission?.RollNo ?? "Student",
                StudentCode = studentInfo?.StdCID ?? "",
                ClassName = admission?.Class?.ClassName ?? studentInfo?.ApplicationForClass ?? "",
                SectionName = admission?.Section?.SectionName ?? "",
                RollNo = admission?.RollNo ?? studentInfo?.StdCID ?? "",
                AcademicYear = sessionTitle,
                TotalUnpaidDueOverall = totalOverallDue,
                AdmitCards = admitCards
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<StudentAdmitCardListResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private static decimal CalculateDueSum(IEnumerable<Domain.Entities.BillMaster> bills)
    {
        decimal sum = 0;
        foreach (var b in bills)
        {
            decimal due = b.DueAmount > 0 ? b.DueAmount : (b.TotalAmount - b.CollectionAmount);
            if (due <= 0 && !b.IsPaid) due = b.TotalAmount;
            sum += due;
        }
        return sum;
    }
}

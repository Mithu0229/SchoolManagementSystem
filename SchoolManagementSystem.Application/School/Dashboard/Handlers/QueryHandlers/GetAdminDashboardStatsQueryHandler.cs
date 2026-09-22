using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Dashboard.Models;
using SchoolManagementSystem.Application.School.Dashboard.Queries;
using System.Globalization;

namespace SchoolManagementSystem.Application.School.Dashboard.Handlers.QueryHandlers;

public class GetAdminDashboardStatsQueryHandler : IHttpRequestHandler<GetAdminDashboardStatsQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAdminDashboardStatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var studentInfoCount = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted(false,true).CountAsync(cancellationToken);
            //var studentCount = await _unitOfWork.StudentRepository.GetAllNoneDeleted(true).CountAsync(cancellationToken);
            var totalStudents = Math.Max(studentInfoCount, 0);

            var feeCollectionsQuery = _unitOfWork.BillMasterRepository.GetAllNoneDeleted(false,true).Where(x => !x.IsPaid);
            var totalRevenue = await feeCollectionsQuery.SumAsync(x => (decimal?)x.TotalAmount, cancellationToken) ?? 0m;
            var totalDue = await feeCollectionsQuery.SumAsync(x => (decimal?)x.DueAmount, cancellationToken) ?? 0m;

            var admissionsQuery = _unitOfWork.AdmissionRepository.GetAllNoneDeleted(true).Where(x => !x.IsCancelled);
            var totalAdmissions = await admissionsQuery.CountAsync(cancellationToken);

            var totalTeachers = await _unitOfWork.TeacherRepository.GetAllNoneDeleted(true).CountAsync(cancellationToken);

            // Last 6 months revenue trend
            var now = DateTime.UtcNow;
            var sixMonthsAgo = new DateTime(now.Year, now.Month, 1).AddMonths(-5);
            var monthlyData = await feeCollectionsQuery
                    .Where(x => x.PaymentDate.HasValue &&
                                x.PaymentDate.Value >= sixMonthsAgo)
                    .GroupBy(x => new
                    {
                        Year = x.PaymentDate.Value.Year,
                        Month = x.PaymentDate.Value.Month
                    })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Revenue = g.Sum(x => x.TotalAmount),
                        Due = (g.Sum(x => x.TotalAmount) -g.Sum(x => x.CollectionAmount))
                    })
                    .ToListAsync(cancellationToken);

            var revenueOverview = new List<MonthlyRevenueDto>();
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = now.AddMonths(-i);
                var found = monthlyData.FirstOrDefault(m => m.Year == targetDate.Year && m.Month == targetDate.Month);
                revenueOverview.Add(new MonthlyRevenueDto
                {
                    Month = targetDate.ToString("MMM", CultureInfo.InvariantCulture),
                    Revenue = found?.Revenue ?? 0m,
                    Expenses = found?.Due ?? 0m
                });
            }

            // Student Demographics by Class
            var classGroups = await admissionsQuery
                .Where(x => x.Class != null)
                .GroupBy(x => x.Class.ClassName)
                .Select(g => new ClassDemographicsDto
                {
                    ClassName = g.Key,
                    StudentCount = g.Count()
                })
                .OrderByDescending(x => x.StudentCount)
                .Take(5)
                .ToListAsync(cancellationToken);

            // Recent Admissions
            var recentAdmissions = await admissionsQuery
                .OrderByDescending(x => x.AdmissionDate)
                .Take(6)
                .Select(x => new RecentAdmissionDto
                {
                    Id = x.Id,
                    StudentName = x.Student != null ? x.Student.FullName ?? "Student" : "Student",
                    ClassName = x.Class != null ? x.Class.ClassName : "N/A",
                    RollNo = x.RollNo,
                    AdmissionDate = x.AdmissionDate,
                    Status = x.IsPassed ? "Passed" : "Enrolled"
                })
                .ToListAsync(cancellationToken);

            var response = new AdminDashboardStatsResponse
            {
                TotalStudents = totalStudents,
                TotalRevenue = totalRevenue,
                TotalAdmissions = totalAdmissions,
                TotalTeachers = totalTeachers,
                TotalDue = totalDue,
                RevenueOverview = revenueOverview,
                StudentDemographics = classGroups,
                RecentAdmissions = recentAdmissions
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<AdminDashboardStatsResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

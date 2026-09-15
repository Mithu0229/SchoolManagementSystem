using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetStudentPaidBillReportQuery(Guid StudentId) : IHttpRequest;

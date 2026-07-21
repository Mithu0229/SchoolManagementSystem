using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetPaidFeesByStudentIdQuery(Guid StudentId, bool IsActive) : IHttpRequest;

using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetFeeInfoByStudentIdQuery(Guid StudentId) : IHttpRequest;

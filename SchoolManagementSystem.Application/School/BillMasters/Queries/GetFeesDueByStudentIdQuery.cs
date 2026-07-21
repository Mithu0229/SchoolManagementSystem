using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.BillMasters.Queries;

public record GetFeesDueByStudentIdQuery(Guid StudentId, int Month, int Year) : IHttpRequest;

using SchoolManagementSystem.Application.Common;

namespace SchoolManagementSystem.Application.School.Subjects.Queries;

public class GetSubjectListQuery : IHttpRequest
{
    public PagedRequest PagedRequest { get; set; } = new();
}

public class GetSubjectByIdQuery(Guid id) : IHttpRequest
{
    public Guid Id { get; set; } = id;
}

public class GetSubjectDropdownQuery : IHttpRequest
{
}

public class GetSubjectsByClassQuery(Guid classId) : IHttpRequest
{
    public Guid ClassId { get; set; } = classId;
}

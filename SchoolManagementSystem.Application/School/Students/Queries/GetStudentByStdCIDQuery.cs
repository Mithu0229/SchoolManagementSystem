namespace SchoolManagementSystem.Application.School.Students.Queries;
public record GetStudentByStdCIDQuery(string StdCID) : IHttpRequest;

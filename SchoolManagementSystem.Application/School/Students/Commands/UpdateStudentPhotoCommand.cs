namespace SchoolManagementSystem.Application.School.Students.Commands;

public class UpdateStudentPhotoCommand : IHttpRequest
{
    public Guid StudentId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}

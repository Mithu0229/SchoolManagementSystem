namespace SchoolManagementSystem.Application.School.SchoolStudents.Models;

public class StudentResponse
{
    public Guid Id { get; set; }
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? MobileNo { get; set; }
    public string? Email { get; set; }
    public string? DOBNo { get; set; }
    public string? GuardianNID { get; set; }
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? GuardianMobileNo { get; set; }
    public string? PresentAddress { get; set; }
    public string? PermanentAddress { get; set; }
    public string? PhotoPath { get; set; }
    public bool IsActive { get; set; }
}

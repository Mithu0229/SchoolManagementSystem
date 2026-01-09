using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Application.School.Students.Models;

public class StudentInfoResponse
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public int Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public   string PlaceOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Religion { get; set; }
    public   string BloodGroup { get; set; }
    public   string BirthCertificateNo { get; set; }
    public string? ApplicationForClass { get; set; }
    public string? AcademicYear { get; set; }
    public string? LastSchool { get; set; }
    public string? LastClassAttendedResult { get; set; }
    public string? IsDisability { get; set; }
    public string? Disability { get; set; }
    public string? SpecialCare { get; set; }
    public   string PresentAddress { get; set; }
    public   string PermanentAddress { get; set; }
    public string? StudentPhone { get; set; }
    public string? StudentEmail { get; set; }

    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? Name { get; set; }

    public GuardianInfo? GuardianInfo { get; set; }
    public LocalGuardianInfo? LocalGuardianInfo { get; set; }
}

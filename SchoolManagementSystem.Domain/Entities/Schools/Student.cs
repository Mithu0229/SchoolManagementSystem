namespace SchoolManagementSystem.Domain.Entities;

public class Student : AuditableEntity
{
    public required string StudentCode { get; set; }
    public required string StudentName { get; set; }
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
}

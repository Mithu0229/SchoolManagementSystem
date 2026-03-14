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
    public string? ImagePath { get; set; }

    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? Name { get; set; }

    public GuardianInfoResponse? GuardianInfo { get; set; }
    public LocalGuardianInfoResponse? LocalGuardianInfo { get; set; }
}

public class GuardianInfoResponse{
    public Guid Id { get; set; }
    public string FatherName { get; set; }
    public string? FatherAcademicQualification { get; set; }
    public string? FatherOccupation { get; set; }
    public string FatherNationalIdNo { get; set; }
    public string FatherMobile { get; set; }
    public string? FatherTelephoneOffice { get; set; }
    public string? FatherTelephoneResidence { get; set; }
    public string? FatherEmail { get; set; }

    public string MotherName { get; set; }
    public string? MotherAcademicQualification { get; set; }
    public string? MotherOccupation { get; set; }
    public string MotherNationalIdNo { get; set; }
    public string? MotherMobile { get; set; }
    public string? MotherTelephoneOffice { get; set; }
    public string? MotherTelephoneResidence { get; set; }
    public string? MotherEmail { get; set; }

    public Guid StudentInfoId { get; set; }
}

public class LocalGuardianInfoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string RelationToStudent { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
    public Guid StudentInfoId { get; set; }
}

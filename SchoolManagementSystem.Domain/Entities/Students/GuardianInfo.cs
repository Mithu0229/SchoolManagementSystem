namespace SchoolManagementSystem.Domain.Entities.Students;

public class GuardianInfo : CoreEntity
{
    public string FatherName { get; set; }
    public string? FatherAcademicQualification { get; set; }
    public string? FatherOccupation { get; set; }
    public string? FatherNationalIdNo { get; set; }
    public string FatherMobile { get; set; }
    public string? FatherTelephoneOffice { get; set; }
    public string? FatherTelephoneResidence { get; set; }
    public string? FatherEmail { get; set; }

    public string MotherName { get; set; }
    public string? MotherAcademicQualification { get; set; }
    public string? MotherOccupation { get; set; }
    public string? MotherNationalIdNo { get; set; }
    public string? MotherMobile { get; set; }
    public string? MotherTelephoneOffice { get; set; }
    public string? MotherTelephoneResidence { get; set; }
    public string? MotherEmail { get; set; }

    public Guid StudentInfoId { get; set; }
    public virtual StudentInfo? StudentInfo { get; set; }
}

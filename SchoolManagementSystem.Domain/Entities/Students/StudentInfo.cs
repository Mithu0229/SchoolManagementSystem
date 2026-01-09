namespace SchoolManagementSystem.Domain.Entities.Students;

public class StudentInfo : TenantEntity
{
    public  string? FullName { get; set; }
    public  int Gender { get; set; }
    public  DateTime DateOfBirth { get; set; }
    public  string PlaceOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Religion { get; set; }
    public  string BloodGroup { get; set; }
    public  string BirthCertificateNo { get; set; }
    public string? ApplicationForClass { get; set; }
    public string? AcademicYear { get; set; }
    public string? LastSchool { get; set; }
    public string? LastClassAttendedResult { get; set; }
    public string? IsDisability { get; set; }
    public string? Disability { get; set; }
    public string? SpecialCare { get; set; }
    public  string PresentAddress { get; set; }
    public  string PermanentAddress { get; set; }
    public string? StudentPhone { get; set; }
    public string? StudentEmail { get; set; }
    public string? ImagePath { get; set; }
    //public Guid? GuardianInfoId { get; set; }
    //public Guid? LocalGuardianInfoId { get; set; }
    public virtual GuardianInfo? GuardianInfo { get; set; }
    public virtual LocalGuardianInfo? LocalGuardianInfo { get; set; }

    
}

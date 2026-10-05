namespace SchoolManagementSystem.Application.School.Students.Excel;

/// <summary>
/// Describes one column of the Student import Excel file.
/// </summary>
/// <param name="Key">Stable key used by the parser (maps to a StudentInfoRequest property).</param>
/// <param name="Header">Header text written in the sample file (without the required marker).</param>
/// <param name="Required">Whether the value is mandatory (mirrors the manual admission form + DB constraints).</param>
/// <param name="Group">Logical section (Student / Father / Mother / Local Guardian).</param>
/// <param name="Hint">Help text shown as a header comment and on the Instructions sheet.</param>
/// <param name="Example">Example value shown on the Instructions sheet.</param>
/// <param name="IsText">Force Excel "Text" format so values like phone numbers keep their leading zeros.</param>
/// <param name="MaxLength">Optional max length enforced by the database.</param>
public sealed record StudentExcelColumn(
    string Key,
    string Header,
    bool Required,
    string Group,
    string Hint,
    string Example,
    bool IsText = false,
    int? MaxLength = null);

/// <summary>
/// Single source of truth for the Student Excel template. The sample download and the
/// upload parser are both driven by this definition, so they can never drift apart.
/// Fields map 1:1 to <c>StudentInfoRequest</c> / <c>GuardianInfoRequest</c> /
/// <c>LocalGuardianInfoRequest</c> used by <c>StudentInfoController.save-student</c>.
/// </summary>
public static class StudentExcelTemplate
{
    public const string StudentsSheetName = "Students";
    public const string InstructionsSheetName = "Instructions";
    public const string ListsSheetName = "Lists";
    public const string RequiredMarker = " *";

    /// <summary>Maximum number of data rows accepted in one upload.</summary>
    public const int MaxRows = 1000;

    public const string GroupStudent = "Student";
    public const string GroupFather = "Father";
    public const string GroupMother = "Mother";
    public const string GroupLocalGuardian = "Local Guardian";

    // ---- Keys -------------------------------------------------------------------------
    public const string FullName = nameof(FullName);
    public const string Gender = nameof(Gender);
    public const string DateOfBirth = nameof(DateOfBirth);
    public const string PlaceOfBirth = nameof(PlaceOfBirth);
    public const string Nationality = nameof(Nationality);
    public const string Religion = nameof(Religion);
    public const string BloodGroup = nameof(BloodGroup);
    public const string BirthCertificateNo = nameof(BirthCertificateNo);
    public const string ApplicationForClass = nameof(ApplicationForClass);
    public const string AcademicYear = nameof(AcademicYear);
    public const string StudentPhone = nameof(StudentPhone);
    public const string StudentEmail = nameof(StudentEmail);
    public const string PresentAddress = nameof(PresentAddress);
    public const string PermanentAddress = nameof(PermanentAddress);
    public const string LastSchool = nameof(LastSchool);
    public const string LastClassAttendedResult = nameof(LastClassAttendedResult);
    public const string IsDisability = nameof(IsDisability);
    public const string Disability = nameof(Disability);
    public const string SpecialCare = nameof(SpecialCare);

    public const string FatherName = nameof(FatherName);
    public const string FatherMobile = nameof(FatherMobile);
    public const string FatherOccupation = nameof(FatherOccupation);
    public const string FatherAcademicQualification = nameof(FatherAcademicQualification);
    public const string FatherNationalIdNo = nameof(FatherNationalIdNo);
    public const string FatherEmail = nameof(FatherEmail);
    public const string FatherTelephoneOffice = nameof(FatherTelephoneOffice);
    public const string FatherTelephoneResidence = nameof(FatherTelephoneResidence);

    public const string MotherName = nameof(MotherName);
    public const string MotherMobile = nameof(MotherMobile);
    public const string MotherOccupation = nameof(MotherOccupation);
    public const string MotherAcademicQualification = nameof(MotherAcademicQualification);
    public const string MotherNationalIdNo = nameof(MotherNationalIdNo);
    public const string MotherEmail = nameof(MotherEmail);
    public const string MotherTelephoneOffice = nameof(MotherTelephoneOffice);
    public const string MotherTelephoneResidence = nameof(MotherTelephoneResidence);

    public const string LocalGuardianName = nameof(LocalGuardianName);
    public const string LocalGuardianRelation = nameof(LocalGuardianRelation);
    public const string LocalGuardianPhone = nameof(LocalGuardianPhone);
    public const string LocalGuardianEmail = nameof(LocalGuardianEmail);
    public const string LocalGuardianAddress = nameof(LocalGuardianAddress);

    // ---- Allowed values (identical to the manual admission form dropdowns) ------------
    /// <summary>Gender label → stored int value (same as the admission form: 1/2/3).</summary>
    public static readonly IReadOnlyDictionary<string, int> Genders =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            ["Male"] = 1,
            ["Female"] = 2,
            ["Other"] = 3,
        };

    public static readonly IReadOnlyList<string> Religions =
        new[] { "Christianity", "Islam", "Hinduism", "Buddhism", "Sikhism", "Judaism", "Other" };

    public static readonly IReadOnlyList<string> BloodGroups =
        new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

    public static readonly IReadOnlyList<string> YesNo = new[] { "Yes", "No" };

    // ---- Columns (order = column order in the sample file) ----------------------------
    public static readonly IReadOnlyList<StudentExcelColumn> Columns = new List<StudentExcelColumn>
    {
        // Student
        new(FullName, "Full Name", true, GroupStudent, "Student's full name. Also used to create the student's login user, so it must be unique.", "Rahim Uddin", MaxLength: 200),
        new(Gender, "Gender", true, GroupStudent, "Select from list: Male, Female, Other.", "Male"),
        new(DateOfBirth, "Date Of Birth", true, GroupStudent, "Date of birth. Use an Excel date or the format yyyy-MM-dd (e.g. 2015-03-25).", "2015-03-25"),
        new(PlaceOfBirth, "Place Of Birth", true, GroupStudent, "City / district of birth.", "Dhaka"),
        new(Nationality, "Nationality", true, GroupStudent, "Nationality.", "Bangladeshi"),
        new(Religion, "Religion", true, GroupStudent, "Select from list: " + string.Join(", ", Religions) + ".", "Islam"),
        new(BloodGroup, "Blood Group", true, GroupStudent, "Select from list: " + string.Join(", ", BloodGroups) + ".", "B+"),
        new(BirthCertificateNo, "Birth Certificate No", true, GroupStudent, "Birth registration number.", "20151234567890123", IsText: true),
        new(ApplicationForClass, "Application For Class", true, GroupStudent, "Select the class name from the list (must be an existing active class).", "Class One"),
        new(AcademicYear, "Academic Year", true, GroupStudent, "4-digit year, e.g. 2026.", "2026", IsText: true),
        new(StudentPhone, "Student Phone", true, GroupStudent, "Mobile number. Keep leading 0 (column is formatted as text).", "01712345678", IsText: true),
        new(StudentEmail, "Student Email", false, GroupStudent, "Optional email address.", "rahim@example.com"),
        new(PresentAddress, "Present Address", true, GroupStudent, "Current address.", "House 34, Road 8, Mirpur, Dhaka", MaxLength: 1000),
        new(PermanentAddress, "Permanent Address", true, GroupStudent, "Permanent address.", "Village X, Comilla", MaxLength: 1000),
        new(LastSchool, "Last School", false, GroupStudent, "Optional previous school name.", "ABC School"),
        new(LastClassAttendedResult, "Last Class Attended Result", false, GroupStudent, "Optional result of the last class attended.", "GPA 5.00"),
        new(IsDisability, "Is Disability", false, GroupStudent, "Yes or No. Blank = No.", "No"),
        new(Disability, "Disability", false, GroupStudent, "Optional disability details.", ""),
        new(SpecialCare, "Special Care", false, GroupStudent, "Optional special care notes.", ""),

        // Father
        new(FatherName, "Father Name", true, GroupFather, "Father's full name.", "Karim Uddin"),
        new(FatherMobile, "Father Mobile", true, GroupFather, "Father's mobile number (text, keep leading 0).", "01811111111", IsText: true),
        new(FatherOccupation, "Father Occupation", false, GroupFather, "Optional.", "Business"),
        new(FatherAcademicQualification, "Father Academic Qualification", false, GroupFather, "Optional.", "B.Com"),
        new(FatherNationalIdNo, "Father National ID No", false, GroupFather, "Optional NID number.", "1990123456789", IsText: true),
        new(FatherEmail, "Father Email", false, GroupFather, "Optional.", "karim@example.com"),
        new(FatherTelephoneOffice, "Father Telephone Office", false, GroupFather, "Optional.", "", IsText: true),
        new(FatherTelephoneResidence, "Father Telephone Residence", false, GroupFather, "Optional.", "", IsText: true),

        // Mother
        new(MotherName, "Mother Name", true, GroupMother, "Mother's full name.", "Rahima Begum"),
        new(MotherMobile, "Mother Mobile", true, GroupMother, "Mother's mobile number (text, keep leading 0).", "01922222222", IsText: true),
        new(MotherOccupation, "Mother Occupation", false, GroupMother, "Optional.", "Teacher"),
        new(MotherAcademicQualification, "Mother Academic Qualification", false, GroupMother, "Optional.", "M.A."),
        new(MotherNationalIdNo, "Mother National ID No", false, GroupMother, "Optional NID number.", "1992123456789", IsText: true),
        new(MotherEmail, "Mother Email", false, GroupMother, "Optional.", "rahima@example.com"),
        new(MotherTelephoneOffice, "Mother Telephone Office", false, GroupMother, "Optional.", "", IsText: true),
        new(MotherTelephoneResidence, "Mother Telephone Residence", false, GroupMother, "Optional.", "", IsText: true),

        // Local guardian
        new(LocalGuardianName, "Local Guardian Name", false, GroupLocalGuardian, "Optional.", "Jamal Hossain"),
        new(LocalGuardianRelation, "Local Guardian Relation", false, GroupLocalGuardian, "Optional relation to student.", "Uncle"),
        new(LocalGuardianPhone, "Local Guardian Phone", false, GroupLocalGuardian, "Optional (text, keep leading 0).", "01633333333", IsText: true),
        new(LocalGuardianEmail, "Local Guardian Email", false, GroupLocalGuardian, "Optional.", ""),
        new(LocalGuardianAddress, "Local Guardian Address", false, GroupLocalGuardian, "Optional.", "Uttara, Dhaka"),
    };

    /// <summary>Header text as written in the sample file (required columns get " *").</summary>
    public static string DisplayHeader(StudentExcelColumn column)
        => column.Required ? column.Header + RequiredMarker : column.Header;

    /// <summary>Normalizes a header for tolerant matching (case, spaces and '*' ignored).</summary>
    public static string NormalizeHeader(string? header)
        => new string((header ?? string.Empty)
                .Where(c => !char.IsWhiteSpace(c) && c != '*')
                .ToArray())
            .ToLowerInvariant();
}

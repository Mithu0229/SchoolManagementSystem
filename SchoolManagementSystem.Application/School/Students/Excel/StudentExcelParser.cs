using System.Globalization;
using ClosedXML.Excel;
using SchoolManagementSystem.Application.School.Students.Models;

namespace SchoolManagementSystem.Application.School.Students.Excel;

/// <summary>One parsed Excel data row.</summary>
public sealed class StudentExcelParsedRow
{
    public int RowNumber { get; init; }
    public StudentInfoRequest Request { get; init; } = new();
    public List<string> Errors { get; } = new();
    public bool IsValid => Errors.Count == 0;
}

/// <summary>Result of parsing the uploaded workbook.</summary>
public sealed class StudentExcelParseResult
{
    /// <summary>File-level problem (wrong format, missing headers...). When set, Rows is empty.</summary>
    public string? FileError { get; init; }
    public List<StudentExcelParsedRow> Rows { get; init; } = new();
}

/// <summary>
/// Reads the uploaded Student Excel and maps each row to a <see cref="StudentInfoRequest"/>
/// (the exact model accepted by <c>StudentInfoController.save-student</c>), collecting
/// per-row validation errors that mirror the manual admission form rules.
/// </summary>
public static class StudentExcelParser
{
    private static readonly string[] DateFormats =
    {
        "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
        "dd.MM.yyyy", "dd-MMM-yyyy", "d-MMM-yyyy", "dd MMM yyyy", "d MMM yyyy",
        "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-dd HH:mm:ss",
    };

    /// <param name="stream">The .xlsx file content.</param>
    /// <param name="classLookup">Active classes: class name (case-insensitive) → class Id.</param>
    public static StudentExcelParseResult Parse(Stream stream, IReadOnlyDictionary<string, Guid> classLookup)
    {
        XLWorkbook workbook;
        try
        {
            workbook = new XLWorkbook(stream);
        }
        catch (Exception)
        {
            return new StudentExcelParseResult { FileError = "The file could not be read. Please upload a valid .xlsx file created from the sample template." };
        }

        using (workbook)
        {
            if (!workbook.TryGetWorksheet(StudentExcelTemplate.StudentsSheetName, out var ws))
            {
                ws = workbook.Worksheets.FirstOrDefault();
            }
            if (ws == null)
            {
                return new StudentExcelParseResult { FileError = "The workbook does not contain any worksheet." };
            }

            // ---- Map header → column number --------------------------------------------
            var headerRow = ws.Row(1);
            var lastHeaderCol = headerRow.LastCellUsed()?.Address.ColumnNumber ?? 0;
            var headerIndex = new Dictionary<string, int>();
            for (var c = 1; c <= lastHeaderCol; c++)
            {
                var normalized = StudentExcelTemplate.NormalizeHeader(headerRow.Cell(c).GetString());
                if (normalized.Length > 0 && !headerIndex.ContainsKey(normalized))
                {
                    headerIndex[normalized] = c;
                }
            }

            var columnMap = new Dictionary<string, int>();
            var missingRequired = new List<string>();
            foreach (var column in StudentExcelTemplate.Columns)
            {
                if (headerIndex.TryGetValue(StudentExcelTemplate.NormalizeHeader(column.Header), out var colNo))
                {
                    columnMap[column.Key] = colNo;
                }
                else if (column.Required)
                {
                    missingRequired.Add(column.Header);
                }
            }

            if (missingRequired.Count > 0)
            {
                return new StudentExcelParseResult
                {
                    FileError = "The file does not match the sample template. Missing required column(s): "
                                + string.Join(", ", missingRequired)
                                + ". Please download the latest sample file."
                };
            }

            // ---- Rows ----------------------------------------------------------------
            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
            var rows = new List<StudentExcelParsedRow>();
            for (var r = 2; r <= lastRow; r++)
            {
                var xlRow = ws.Row(r);
                if (IsRowEmpty(xlRow, columnMap.Values))
                {
                    continue;
                }

                if (rows.Count >= StudentExcelTemplate.MaxRows)
                {
                    return new StudentExcelParseResult
                    {
                        FileError = $"The file contains more than {StudentExcelTemplate.MaxRows} student rows. Please split it into smaller files."
                    };
                }

                rows.Add(ParseRow(xlRow, r, columnMap, classLookup));
            }

            return new StudentExcelParseResult { Rows = rows };
        }
    }

    // ---------------------------------------------------------------------------------
    private static StudentExcelParsedRow ParseRow(
        IXLRow xlRow,
        int rowNumber,
        IReadOnlyDictionary<string, int> columnMap,
        IReadOnlyDictionary<string, Guid> classLookup)
    {
        var errors = new List<string>();

        string? Text(string key)
        {
            if (!columnMap.TryGetValue(key, out var col)) return null;
            var value = ReadText(xlRow.Cell(col));
            var definition = StudentExcelTemplate.Columns.First(x => x.Key == key);

            if (string.IsNullOrWhiteSpace(value))
            {
                if (definition.Required) errors.Add($"{definition.Header} is required.");
                return null;
            }
            if (definition.MaxLength.HasValue && value.Length > definition.MaxLength.Value)
            {
                errors.Add($"{definition.Header} must not exceed {definition.MaxLength.Value} characters.");
            }
            return value;
        }

        // ---- Student ------------------------------------------------------------------
        var fullName = Text(StudentExcelTemplate.FullName);

        var genderText = Text(StudentExcelTemplate.Gender);
        var gender = 0;
        if (genderText != null)
        {
            if (StudentExcelTemplate.Genders.TryGetValue(genderText, out var g))
                gender = g;
            else if (int.TryParse(genderText, out var gi) && StudentExcelTemplate.Genders.Values.Contains(gi))
                gender = gi;
            else
                errors.Add($"Gender '{genderText}' is invalid. Allowed: {string.Join(", ", StudentExcelTemplate.Genders.Keys)}.");
        }

        DateTime dateOfBirth = default;
        if (columnMap.TryGetValue(StudentExcelTemplate.DateOfBirth, out var dobCol))
        {
            var dobCell = xlRow.Cell(dobCol);
            if (dobCell.IsEmpty() || string.IsNullOrWhiteSpace(dobCell.GetString()))
            {
                errors.Add("Date Of Birth is required.");
            }
            else if (!TryReadDate(dobCell, out dateOfBirth))
            {
                errors.Add($"Date Of Birth '{dobCell.GetString()}' is not a valid date. Use yyyy-MM-dd.");
            }
            else if (dateOfBirth.Date > DateTime.Today || dateOfBirth.Year < 1900)
            {
                errors.Add("Date Of Birth must be between 1900 and today.");
            }
        }

        var placeOfBirth = Text(StudentExcelTemplate.PlaceOfBirth);
        var nationality = Text(StudentExcelTemplate.Nationality);

        var religion = Text(StudentExcelTemplate.Religion);
        if (religion != null)
        {
            var match = StudentExcelTemplate.Religions.FirstOrDefault(x => string.Equals(x, religion, StringComparison.OrdinalIgnoreCase));
            if (match == null) errors.Add($"Religion '{religion}' is invalid. Allowed: {string.Join(", ", StudentExcelTemplate.Religions)}.");
            else religion = match;
        }

        var bloodGroup = Text(StudentExcelTemplate.BloodGroup);
        if (bloodGroup != null)
        {
            var compact = bloodGroup.Replace(" ", "");
            var match = StudentExcelTemplate.BloodGroups.FirstOrDefault(x => string.Equals(x, compact, StringComparison.OrdinalIgnoreCase));
            if (match == null) errors.Add($"Blood Group '{bloodGroup}' is invalid. Allowed: {string.Join(", ", StudentExcelTemplate.BloodGroups)}.");
            else bloodGroup = match;
        }

        var birthCertificateNo = Text(StudentExcelTemplate.BirthCertificateNo);

        // Manual form stores the AcademicClass Id (GUID) in ApplicationForClass.
        string? applicationForClass = null;
        var className = Text(StudentExcelTemplate.ApplicationForClass);
        if (className != null)
        {
            if (classLookup.TryGetValue(className, out var classId))
                applicationForClass = classId.ToString();
            else if (Guid.TryParse(className, out var guid) && classLookup.Values.Contains(guid))
                applicationForClass = guid.ToString();
            else
                errors.Add($"Application For Class '{className}' was not found among active classes.");
        }

        var academicYear = Text(StudentExcelTemplate.AcademicYear);
        if (academicYear != null && (!int.TryParse(academicYear, out var year) || year < 1900 || year > 2100))
        {
            errors.Add($"Academic Year '{academicYear}' is invalid. Use a 4-digit year, e.g. {DateTime.Today.Year}.");
        }

        var studentPhone = Text(StudentExcelTemplate.StudentPhone);
        var studentEmail = Text(StudentExcelTemplate.StudentEmail);
        var presentAddress = Text(StudentExcelTemplate.PresentAddress);
        var permanentAddress = Text(StudentExcelTemplate.PermanentAddress);
        var lastSchool = Text(StudentExcelTemplate.LastSchool);
        var lastClassResult = Text(StudentExcelTemplate.LastClassAttendedResult);

        var isDisabilityText = Text(StudentExcelTemplate.IsDisability);
        var isDisability = false; // manual form default
        if (isDisabilityText != null)
        {
            switch (isDisabilityText.Trim().ToLowerInvariant())
            {
                case "yes": case "y": case "true": case "1": isDisability = true; break;
                case "no": case "n": case "false": case "0": isDisability = false; break;
                default: errors.Add($"Is Disability '{isDisabilityText}' is invalid. Use Yes or No."); break;
            }
        }

        var request = new StudentInfoRequest
        {
            FullName = fullName,
            Gender = gender,
            DateOfBirth = dateOfBirth.Date,
            PlaceOfBirth = placeOfBirth!,
            Nationality = nationality,
            Religion = religion,
            BloodGroup = bloodGroup!,
            BirthCertificateNo = birthCertificateNo!,
            ApplicationForClass = applicationForClass,
            AcademicYear = academicYear,
            LastSchool = lastSchool,
            LastClassAttendedResult = lastClassResult,
            IsDisability = isDisability,
            Disability = Text(StudentExcelTemplate.Disability),
            SpecialCare = Text(StudentExcelTemplate.SpecialCare),
            PresentAddress = presentAddress!,
            PermanentAddress = permanentAddress!,
            StudentPhone = studentPhone,
            StudentEmail = studentEmail,

            // Same nested objects the manual form always posts (GuardianInfo.* / LocalGuardianInfo.*).
            GuardianInfo = new GuardianInfoRequest
            {
                FatherName = Text(StudentExcelTemplate.FatherName)!,
                FatherMobile = Text(StudentExcelTemplate.FatherMobile)!,
                FatherOccupation = Text(StudentExcelTemplate.FatherOccupation),
                FatherAcademicQualification = Text(StudentExcelTemplate.FatherAcademicQualification),
                FatherNationalIdNo = Text(StudentExcelTemplate.FatherNationalIdNo),
                FatherEmail = Text(StudentExcelTemplate.FatherEmail),
                FatherTelephoneOffice = Text(StudentExcelTemplate.FatherTelephoneOffice),
                FatherTelephoneResidence = Text(StudentExcelTemplate.FatherTelephoneResidence),

                MotherName = Text(StudentExcelTemplate.MotherName)!,
                MotherMobile = Text(StudentExcelTemplate.MotherMobile),
                MotherOccupation = Text(StudentExcelTemplate.MotherOccupation),
                MotherAcademicQualification = Text(StudentExcelTemplate.MotherAcademicQualification),
                MotherNationalIdNo = Text(StudentExcelTemplate.MotherNationalIdNo),
                MotherEmail = Text(StudentExcelTemplate.MotherEmail),
                MotherTelephoneOffice = Text(StudentExcelTemplate.MotherTelephoneOffice),
                MotherTelephoneResidence = Text(StudentExcelTemplate.MotherTelephoneResidence),
            },
            LocalGuardianInfo = new LocalGuardianInfoRequest
            {
                Name = Text(StudentExcelTemplate.LocalGuardianName),
                RelationToStudent = Text(StudentExcelTemplate.LocalGuardianRelation),
                Phone = Text(StudentExcelTemplate.LocalGuardianPhone),
                Email = Text(StudentExcelTemplate.LocalGuardianEmail),
                Address = Text(StudentExcelTemplate.LocalGuardianAddress),
            },
        };

        var parsed = new StudentExcelParsedRow { RowNumber = rowNumber, Request = request };
        parsed.Errors.AddRange(errors);
        return parsed;
    }

    private static bool IsRowEmpty(IXLRow row, IEnumerable<int> columns)
        => columns.All(c => string.IsNullOrWhiteSpace(row.Cell(c).GetString()));

    /// <summary>
    /// Reads a cell as trimmed text. Whole numbers are written without decimals / exponent
    /// (e.g. a phone typed into a non-text cell), so no "1.7E+10" values reach the database.
    /// </summary>
    private static string? ReadText(IXLCell cell)
    {
        if (cell.IsEmpty()) return null;
        var value = cell.Value;

        if (value.IsNumber)
        {
            var number = value.GetNumber();
            return Math.Abs(number % 1) < double.Epsilon
                ? number.ToString("0", CultureInfo.InvariantCulture)
                : number.ToString(CultureInfo.InvariantCulture);
        }
        if (value.IsDateTime)
        {
            return value.GetDateTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        var text = cell.GetString()?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    private static bool TryReadDate(IXLCell cell, out DateTime date)
    {
        var value = cell.Value;
        if (value.IsDateTime)
        {
            date = value.GetDateTime();
            return true;
        }
        if (value.IsNumber)
        {
            try
            {
                date = DateTime.FromOADate(value.GetNumber());
                return true;
            }
            catch (ArgumentException)
            {
                date = default;
                return false;
            }
        }

        var text = cell.GetString().Trim();
        return DateTime.TryParseExact(text, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }
}

using ClosedXML.Excel;

namespace SchoolManagementSystem.Application.School.Students.Excel;

/// <summary>
/// Generates the downloadable Student sample Excel (.xlsx) from <see cref="StudentExcelTemplate"/>.
/// </summary>
public static class StudentExcelTemplateBuilder
{
    private static readonly Dictionary<string, XLColor> GroupColors = new()
    {
        [StudentExcelTemplate.GroupStudent] = XLColor.FromHtml("#1E3A8A"),
        [StudentExcelTemplate.GroupFather] = XLColor.FromHtml("#065F46"),
        [StudentExcelTemplate.GroupMother] = XLColor.FromHtml("#9D174D"),
        [StudentExcelTemplate.GroupLocalGuardian] = XLColor.FromHtml("#92400E"),
    };

    public static byte[] Build(IReadOnlyList<string> classNames)
    {
        using var workbook = new XLWorkbook();

        var students = workbook.Worksheets.Add(StudentExcelTemplate.StudentsSheetName);
        var instructions = workbook.Worksheets.Add(StudentExcelTemplate.InstructionsSheetName);
        var lists = workbook.Worksheets.Add(StudentExcelTemplate.ListsSheetName);

        var listRanges = BuildListsSheet(lists, classNames);
        BuildStudentsSheet(students, listRanges);
        BuildInstructionsSheet(instructions, classNames);

        lists.Hide();
        students.SetTabActive();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // ---------------------------------------------------------------------------------
    private static Dictionary<string, IXLRange?> BuildListsSheet(IXLWorksheet ws, IReadOnlyList<string> classNames)
    {
        IXLRange? WriteList(int col, string title, IReadOnlyList<string> values)
        {
            ws.Cell(1, col).Value = title;
            ws.Cell(1, col).Style.Font.Bold = true;
            for (var i = 0; i < values.Count; i++)
            {
                ws.Cell(i + 2, col).SetValue(values[i]);
            }
            return values.Count == 0 ? null : ws.Range(2, col, values.Count + 1, col);
        }

        return new Dictionary<string, IXLRange?>
        {
            [StudentExcelTemplate.Gender] = WriteList(1, "Gender", StudentExcelTemplate.Genders.Keys.ToList()),
            [StudentExcelTemplate.Religion] = WriteList(2, "Religion", StudentExcelTemplate.Religions),
            [StudentExcelTemplate.BloodGroup] = WriteList(3, "Blood Group", StudentExcelTemplate.BloodGroups),
            [StudentExcelTemplate.IsDisability] = WriteList(4, "Yes/No", StudentExcelTemplate.YesNo),
            [StudentExcelTemplate.ApplicationForClass] = WriteList(5, "Class", classNames),
        };
    }

    private static void BuildStudentsSheet(IXLWorksheet ws, Dictionary<string, IXLRange?> listRanges)
    {
        var lastDataRow = StudentExcelTemplate.MaxRows + 1;
        var columns = StudentExcelTemplate.Columns;

        for (var i = 0; i < columns.Count; i++)
        {
            var colNo = i + 1;
            var column = columns[i];
            var header = ws.Cell(1, colNo);

            header.Value = StudentExcelTemplate.DisplayHeader(column);
            header.Style.Font.Bold = true;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Fill.BackgroundColor = GroupColors[column.Group];
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            header.Style.Alignment.WrapText = true;
            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            header.Style.Border.OutsideBorderColor = XLColor.White;
            header.CreateComment().AddText($"{column.Group}{(column.Required ? " (Required)" : " (Optional)")}: {column.Hint}");

            var dataRange = ws.Range(2, colNo, lastDataRow, colNo);

            if (column.IsText)
            {
                // Keep leading zeros for phone / NID / certificate numbers.
                dataRange.Style.NumberFormat.Format = "@";
            }

            if (column.Key == StudentExcelTemplate.DateOfBirth)
            {
                dataRange.Style.DateFormat.Format = "yyyy-mm-dd";
                var dv = dataRange.CreateDataValidation();
                dv.Date.Between(new DateTime(1900, 1, 1), DateTime.Today);
                dv.ErrorTitle = "Invalid date";
                dv.ErrorMessage = "Enter a valid date of birth (yyyy-mm-dd), not in the future.";
                dv.ErrorStyle = XLErrorStyle.Stop;
            }

            if (listRanges.TryGetValue(column.Key, out var listRange) && listRange != null)
            {
                var dv = dataRange.CreateDataValidation();
                dv.List(listRange, true);
                dv.IgnoreBlanks = true;
                dv.ErrorTitle = $"Invalid {column.Header}";
                dv.ErrorMessage = $"Please select a {column.Header} from the dropdown list.";
                dv.ErrorStyle = XLErrorStyle.Stop;
            }

            ws.Column(colNo).Width = Math.Max(16, column.Header.Length + 6);
        }

        ws.Row(1).Height = 32;
        ws.SheetView.FreezeRows(1);
        ws.SheetView.FreezeColumns(1);
        ws.Range(1, 1, 1, columns.Count).SetAutoFilter();
    }

    private static void BuildInstructionsSheet(IXLWorksheet ws, IReadOnlyList<string> classNames)
    {
        ws.Cell(1, 1).Value = "Student Bulk Upload - Instructions";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 16;
        ws.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#1E3A8A");

        var notes = new[]
        {
            $"1. Fill one student per row in the '{StudentExcelTemplate.StudentsSheetName}' sheet, starting at row 2. Do not rename or remove the header row.",
            "2. Columns marked with * are required. Use the dropdowns for Gender, Religion, Blood Group, Application For Class and Is Disability.",
            "3. Date Of Birth: use an Excel date or yyyy-MM-dd (e.g. 2015-03-25).",
            "4. Phone / NID / Birth Certificate columns are formatted as text so leading zeros are kept.",
            "5. Full Name is used to create the student's login user (FullName without spaces + @gmail.com), so names must be unique.",
            $"6. Maximum {StudentExcelTemplate.MaxRows} students per file. Only .xlsx files are accepted.",
            "7. The whole file is validated first. If any row has an error, nothing is saved and the errors are shown row by row.",
            "8. Student photo cannot be uploaded via Excel; add it later from the student edit screen.",
        };
        for (var i = 0; i < notes.Length; i++)
        {
            ws.Cell(i + 3, 1).Value = notes[i];
        }

        var tableStart = notes.Length + 5;
        var headers = new[] { "Column", "Section", "Required", "Description", "Example" };
        for (var c = 0; c < headers.Length; c++)
        {
            var cell = ws.Cell(tableStart, c + 1);
            cell.Value = headers[c];
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A8A");
        }

        var row = tableStart + 1;
        foreach (var column in StudentExcelTemplate.Columns)
        {
            ws.Cell(row, 1).Value = StudentExcelTemplate.DisplayHeader(column);
            ws.Cell(row, 2).Value = column.Group;
            ws.Cell(row, 3).Value = column.Required ? "Yes" : "No";
            ws.Cell(row, 4).Value = column.Hint;
            ws.Cell(row, 5).SetValue(column.Example);
            if (column.Required)
            {
                ws.Cell(row, 3).Style.Font.FontColor = XLColor.FromHtml("#B91C1C");
                ws.Cell(row, 3).Style.Font.Bold = true;
            }
            row++;
        }
        ws.Range(tableStart, 1, row - 1, headers.Length).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws.Range(tableStart, 1, row - 1, headers.Length).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        row += 2;
        ws.Cell(row, 1).Value = "Available classes";
        ws.Cell(row, 1).Style.Font.Bold = true;
        row++;
        if (classNames.Count == 0)
        {
            ws.Cell(row, 1).Value = "No active classes found. Please create classes before uploading students.";
            ws.Cell(row, 1).Style.Font.FontColor = XLColor.FromHtml("#B91C1C");
        }
        foreach (var className in classNames)
        {
            ws.Cell(row++, 1).SetValue(className);
        }

        ws.Column(1).Width = 34;
        ws.Column(2).Width = 16;
        ws.Column(3).Width = 10;
        ws.Column(4).Width = 80;
        ws.Column(5).Width = 34;
    }
}

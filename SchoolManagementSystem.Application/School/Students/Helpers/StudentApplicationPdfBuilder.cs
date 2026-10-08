using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SchoolManagementSystem.Domain.Entities.Students;
using System;
using System.IO;
using System.Linq;

namespace SchoolManagementSystem.Application.School.Students.Helpers;

public static class StudentApplicationPdfBuilder
{
    public static byte[] GeneratePdf(StudentInfo student)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                page.Header().Element(ComposeHeader);
                page.Content().Element(x => ComposeContent(x, student));
                page.Footer().Element(ComposeFooter);
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        return ms.ToArray();
    }

    private static void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            // Add Logo
            row.ConstantItem(60).Height(60).PaddingRight(10).AlignLeft().AlignMiddle().Element(c =>
            {
                // Look for the logo in multiple possible locations (Production vs Local Dev)
                string[] possiblePaths = {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "assets", "favicon.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "favicon.png"),
                    Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "src", "assets", "favicon.png"),
                    @"D:\Company\Eninas\Projects\Noman\SchoolManagementSystem\ClientApp\src\assets\favicon.png"
                };

                string logoPath = possiblePaths.FirstOrDefault(File.Exists);

                if (logoPath != null)
                {
                    c.Image(logoPath);
                }
                else
                {
                    c.Text("LOGO").FontSize(10);
                }
            });

            row.RelativeItem().AlignMiddle().Column(column =>
            {
                column.Item().Text("Edugates Integrated School").FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                column.Item().Text("Faith - Wisdom - Revival - Impact").FontSize(10).FontColor(Colors.Grey.Medium);
            });

            row.ConstantItem(150).AlignRight().AlignMiddle().Column(column =>
            {
                column.Item().Text("ADMISSION APPLICATION").FontSize(14).Bold().FontColor(Colors.Red.Medium);
            });
        });
    }

    private static void ComposeContent(IContainer container, StudentInfo student)
    {
        
        container.PaddingVertical(1, Unit.Centimetre).Column(column =>
        {
            column.Spacing(20);

            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("STUDENT NAME").FontSize(8).FontColor(Colors.Grey.Medium);
                    col.Item().Text(string.IsNullOrEmpty(student.FullName) ? "-" : student.FullName).Bold();
                });
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("APPLYING FOR CLASS").FontSize(8).FontColor(Colors.Grey.Medium);
                    col.Item().Text(string.IsNullOrEmpty(student.ApplicationForClass) ? "-" : student.ApplicationForClass).Bold();
                });
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("STATUS").FontSize(8).FontColor(Colors.Grey.Medium);
                    col.Item().Row(r => 
                    {
                        r.AutoItem().PaddingRight(4).AlignMiddle().Text("●").FontColor(Colors.Red.Medium).FontSize(10);
                        r.RelativeItem().AlignMiddle().Text("New").Bold().FontColor(Colors.Red.Medium);
                    });
                });
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("SUBMITTED ON").FontSize(8).FontColor(Colors.Grey.Medium);
                    col.Item().Text(DateTime.Now.ToString("MMM dd, yyyy - h:mm tt")).Bold();
                });
            });

            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            column.Item().Grid(grid =>
            {
                grid.Columns(2);
                grid.Spacing(15);

                // Student Info section
                grid.Item().Column(col =>
                {
                    col.Item().PaddingBottom(5).Text("STUDENT INFORMATION").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Full Name", student.FullName);
                    AddDetailRow(col, "Gender", student.Gender == 1 ? "Male" : (student.Gender == 2 ? "Female" : "Other"));
                    AddDetailRow(col, "Date of Birth", student.DateOfBirth == default ? "-" : student.DateOfBirth.ToString("dd MMM yyyy"));
                    AddDetailRow(col, "Blood Group", student.BloodGroup);
                    AddDetailRow(col, "Nationality", student.Nationality);
                    AddDetailRow(col, "Religion", student.Religion);
                    AddDetailRow(col, "Birth Certificate No.", student.BirthCertificateNo);
                });

                // Academic Details section
                grid.Item().Column(col =>
                {
                    col.Item().PaddingBottom(5).Text("ACADEMIC DETAILS").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Class Applying For", student.ApplicationForClass);
                    AddDetailRow(col, "Academic Year", student.AcademicYear);
                    AddDetailRow(col, "Last Institute Attended", student.LastSchool);
                    AddDetailRow(col, "Last Class Completed", "-"); // Not in model yet
                    AddDetailRow(col, "Last Result/GPA", student.LastClassAttendedResult);
                    AddDetailRow(col, "Reason for Leaving", "-"); // Not in model yet
                });
                
                // Father's Information
                grid.Item().Column(col =>
                {
                    col.Item().PaddingTop(10).PaddingBottom(5).Text("FATHER's INFORMATION").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Father's Name", student.GuardianInfo?.FatherName);
                    AddDetailRow(col, "Father's Phone", student.GuardianInfo?.FatherMobile);
                    AddDetailRow(col, "Father's Email", student.GuardianInfo?.FatherEmail);
                    AddDetailRow(col, "Father's Education", student.GuardianInfo?.FatherAcademicQualification);
                });

                // Mother's Information
                grid.Item().Column(col =>
                {
                    col.Item().PaddingTop(10).PaddingBottom(5).Text("MOTHER's INFORMATION").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Mother's Name", student.GuardianInfo?.MotherName);
                    AddDetailRow(col, "Mother's Phone", student.GuardianInfo?.MotherMobile);
                    AddDetailRow(col, "Mother's Email", student.GuardianInfo?.MotherEmail);
                    AddDetailRow(col, "Mother's Education", student.GuardianInfo?.MotherAcademicQualification);
                });

                // Guardian / Contact
                grid.Item().Column(col =>
                {
                    col.Item().PaddingTop(10).PaddingBottom(5).Text("GUARDIAN / CONTACT").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Guardian Type", "-"); // Not in model directly
                    AddDetailRow(col, "Guardian Name", student.LocalGuardianInfo?.Name);
                    AddDetailRow(col, "Relation with Student", student.LocalGuardianInfo?.RelationToStudent);
                    AddDetailRow(col, "Guardian Phone", student.LocalGuardianInfo?.Phone);
                    AddDetailRow(col, "Guardian Email", student.LocalGuardianInfo?.Email);
                    AddDetailRow(col, "Guardian Present Address", student.LocalGuardianInfo?.Address);
                    AddDetailRow(col, "Guardian Permanent Address", "-");
                });

                // Address
                grid.Item().Column(col =>
                {
                    col.Item().PaddingTop(10).PaddingBottom(5).Text("ADDRESS").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Present Address", student.PresentAddress);
                    AddDetailRow(col, "Permanent Address", student.PermanentAddress);
                });

                // Emergency Contact
                grid.Item().Column(col =>
                {
                    col.Item().PaddingTop(10).PaddingBottom(5).Text("EMERGENCY CONTACT").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Emergency Contact Number", student.LocalGuardianInfo?.Phone);
                    AddDetailRow(col, "Emergency Relation", student.LocalGuardianInfo?.RelationToStudent);
                    AddDetailRow(col, "Emergency Contact Name", student.LocalGuardianInfo?.Name);
                });

                // Health & Other
                grid.Item().Column(col =>
                {
                    col.Item().PaddingTop(10).PaddingBottom(5).Text("HEALTH & OTHER").FontSize(9).Bold().FontColor(Colors.Grey.Medium);
                    AddDetailRow(col, "Has Disability?", student.IsDisability);
                    AddDetailRow(col, "Disability Type", student.Disability);
                    AddDetailRow(col, "Regular Medication", "-");
                    AddDetailRow(col, "Special Care Needed", student.SpecialCare);
                    AddDetailRow(col, "Interests / Hobbies", "-");
                    AddDetailRow(col, "Agreed to Parent Charter", "Yes");
                });
            });
            
            column.Item().PaddingTop(30).Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().LineHorizontal(1).LineColor(Colors.Black);
                    c.Item().AlignCenter().Text("Guardian's Signature").FontSize(8);
                });
                row.ConstantItem(100);
                row.RelativeItem().Column(c =>
                {
                    c.Item().LineHorizontal(1).LineColor(Colors.Black);
                    c.Item().AlignCenter().Text("Authorized Signature & Seal").FontSize(8);
                });
            });

        });
    }

    private static void AddDetailRow(ColumnDescriptor col, string label, string value)
    {
        col.Item().PaddingBottom(2).Row(row =>
        {
            row.ConstantItem(120).Text(label).FontSize(9).FontColor(Colors.Grey.Darken2);
            row.RelativeItem().Text(string.IsNullOrEmpty(value) ? "-" : value).FontSize(9);
        });
    }

    private static void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.CurrentPageNumber();
            x.Span(" / ");
            x.TotalPages();
        });
    }
}

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace SchoolManagementSystem.Application.School.AdmitCards.Helpers;

public class AdmitCardRenderModel
{
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public string RollNo { get; set; } = string.Empty;
    public string ExamStarts { get; set; } = string.Empty;
    public string Session { get; set; } = "Session 2026";
    public string ExamTitle { get; set; } = "Class Test (CT) Exam";
    public string ExamType { get; set; } = "CT";
    public string SchoolName { get; set; } = "EdugAtes Integrated School";
    public string Tagline { get; set; } = "FAITH · WISDOM · REVIVAL · IMPACT";
    public string SchoolAddress { get; set; } = "[School address]";
    public string? StudentImagePath { get; set; }
}

public static class AdmitCardPdfBuilder
{
    public static byte[] GeneratePdf(AdmitCardRenderModel model)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        // Determine badge color and header style based on exam type
        var badgeColor = GetBadgeColor(model.ExamType);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(32, Unit.Point);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                page.Content().Element(content => ComposeCardContent(content, model, badgeColor));
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        return ms.ToArray();
    }

    private static string GetBadgeColor(string examType)
    {
        var type = (examType ?? "").ToUpperInvariant().Trim();
        if (type.Contains("CT") || type.Contains("CLASS"))
        {
            return "#0d603a"; // Forest Green
        }
        if (type.Contains("HALF") || type.Contains("SEMESTER"))
        {
            return "#233979"; // Deep Navy
        }
        if (type.Contains("YEAR") || type.Contains("ANNUAL") || type.Contains("FINAL"))
        {
            return "#9b1c2b"; // Crimson Maroon
        }
        return "#233979";
    }

    private static void ComposeCardContent(IContainer container, AdmitCardRenderModel model, string badgeColor)
    {
        var logoBytes = TryLoadLogoBytes();
        var photoBytes = TryLoadPhotoBytes(model.StudentImagePath);

        container.Column(col =>
        {
            // Top Accent Bar (Navy Blue and Green on top right)
            col.Item().PaddingBottom(12).Row(r =>
            {
                r.RelativeItem(7); // Transparent / White space
                r.RelativeItem(2).Height(5).Background("#233979");
                r.RelativeItem(1).Height(5).Background("#0e6b38");
            });

            // Header Section
            col.Item().Row(row =>
            {
                // Left Header: Logo, Tagline, Address
                row.RelativeItem(3).Column(c =>
                {
                    if (logoBytes != null && logoBytes.Length > 0)
                    {
                        c.Item().Height(48).AlignLeft().Image(logoBytes).FitArea();
                    }
                    else
                    {
                        c.Item().Text("EdugAtes Integrated School")
                            .FontSize(20)
                            .Bold()
                            .FontColor("#233979");
                    }

                    c.Item().PaddingTop(2).Text(model.Tagline ?? "FAITH · WISDOM · REVIVAL · IMPACT")
                        .FontSize(8.5f)
                        .Bold()
                        .FontColor("#1b5e52");

                    var address = string.IsNullOrWhiteSpace(model.SchoolAddress) ? "[School address]" : model.SchoolAddress;
                    c.Item().PaddingTop(1).Text(address)
                        .FontSize(8.5f)
                        .FontColor(Colors.Grey.Darken2);
                });

                // Right Header: Admit Card Badge, Exam Title, Session
                row.RelativeItem(2).AlignRight().Column(c =>
                {
                    c.Item().AlignRight().Element(b =>
                    {
                        b.Background(badgeColor)
                         .PaddingVertical(4)
                         .PaddingHorizontal(16)
                         .Text("ADMIT CARD")
                         .FontSize(14)
                         .Bold()
                         .FontColor(Colors.White);
                    });

                    c.Item().PaddingTop(4).AlignRight().Text(model.ExamTitle)
                        .FontSize(14)
                        .Bold()
                        .FontColor("#111827");

                    c.Item().PaddingTop(1).AlignRight().Text(model.Session)
                        .FontSize(9.5f)
                        .FontColor(Colors.Grey.Darken1);
                });
            });

            col.Item().PaddingTop(16);

            // Student Information and Photo Section
            col.Item().Row(mainRow =>
            {
                // Left 78%: Student details
                mainRow.RelativeItem(4).Column(infoCol =>
                {
                    infoCol.Spacing(12);

                    // Student's Name
                    infoCol.Item().Column(c =>
                    {
                        c.Item().Text("STUDENT’S NAME")
                            .FontSize(8)
                            .Bold()
                            .FontColor(Colors.Grey.Darken3);

                        c.Item().PaddingTop(3).Column(nameRow =>
                        {
                            nameRow.Item().Text(string.IsNullOrWhiteSpace(model.StudentName) ? "-" : model.StudentName)
                                .FontSize(11.5f)
                                .Bold()
                                .FontColor("#111827");

                            nameRow.Item().PaddingTop(1).LineHorizontal(0.75f).LineColor(Colors.Grey.Lighten1);
                        });
                    });

                    // Class and Section (2 columns)
                    infoCol.Item().Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("CLASS")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken3);

                            c.Item().PaddingTop(3).Column(valRow =>
                            {
                                valRow.Item().Text(string.IsNullOrWhiteSpace(model.ClassName) ? "-" : model.ClassName)
                                    .FontSize(11)
                                    .SemiBold()
                                    .FontColor("#111827");

                                valRow.Item().PaddingTop(1).LineHorizontal(0.75f).LineColor(Colors.Grey.Lighten1);
                            });
                        });

                        r.ConstantItem(24); // Gap

                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("SECTION")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken3);

                            c.Item().PaddingTop(3).Column(valRow =>
                            {
                                valRow.Item().Text(string.IsNullOrWhiteSpace(model.SectionName) ? "-" : model.SectionName)
                                    .FontSize(11)
                                    .SemiBold()
                                    .FontColor("#111827");

                                valRow.Item().PaddingTop(1).LineHorizontal(0.75f).LineColor(Colors.Grey.Lighten1);
                            });
                        });
                    });

                    // Roll No and Exam Starts (2 columns)
                    infoCol.Item().Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("ROLL NO.")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken3);

                            c.Item().PaddingTop(3).Column(valRow =>
                            {
                                valRow.Item().Text(string.IsNullOrWhiteSpace(model.RollNo) ? "-" : model.RollNo)
                                    .FontSize(11)
                                    .SemiBold()
                                    .FontColor("#111827");

                                valRow.Item().PaddingTop(1).LineHorizontal(0.75f).LineColor(Colors.Grey.Lighten1);
                            });
                        });

                        r.ConstantItem(24); // Gap

                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("EXAM STARTS")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken3);

                            c.Item().PaddingTop(3).Column(valRow =>
                            {
                                valRow.Item().Text(string.IsNullOrWhiteSpace(model.ExamStarts) ? "-" : model.ExamStarts)
                                    .FontSize(11)
                                    .SemiBold()
                                    .FontColor("#111827");

                                valRow.Item().PaddingTop(1).LineHorizontal(0.75f).LineColor(Colors.Grey.Lighten1);
                            });
                        });
                    });
                });

                mainRow.ConstantItem(24); // Gap before photo

                // Right 22%: Photo Box
                mainRow.RelativeItem(1).AlignRight().Column(photoCol =>
                {
                    photoCol.Item().Width(110).Height(135).Border(1).BorderColor(Colors.Grey.Lighten1).Element(box =>
                    {
                        if (photoBytes != null && photoBytes.Length > 0)
                        {
                            box.Image(photoBytes).FitArea();
                        }
                        else
                        {
                            box.AlignCenter().AlignMiddle().Column(txt =>
                            {
                                txt.Item().AlignCenter().Text("Student").FontSize(9).FontColor(Colors.Grey.Darken1);
                                txt.Item().AlignCenter().Text("Photo").FontSize(9).FontColor(Colors.Grey.Darken1);
                            });
                        }
                    });
                });
            });

            col.Item().PaddingTop(18);

            // Instructions Box
            col.Item().Background("#eff6ff").Padding(10).Column(instBox =>
            {
                instBox.Item().PaddingBottom(4).Text("INSTRUCTIONS")
                    .FontSize(8.5f)
                    .Bold()
                    .FontColor("#1d4ed8");

                instBox.Item().Row(r =>
                {
                    r.RelativeItem().Column(c =>
                    {
                        c.Item().Text("• Bring this card to every exam. No entry without it.")
                            .FontSize(8.5f).FontColor("#1f2937");
                        c.Item().PaddingTop(2).Text("• Mobile phones and notes are not allowed in the hall.")
                            .FontSize(8.5f).FontColor("#1f2937");
                    });

                    r.ConstantItem(20);

                    r.RelativeItem().Column(c =>
                    {
                        c.Item().Text("• Arrive 15 minutes before the exam starts.")
                            .FontSize(8.5f).FontColor("#1f2937");
                        c.Item().PaddingTop(2).Text("• Keep this card clean and do not write on it.")
                            .FontSize(8.5f).FontColor("#1f2937");
                    });
                });
            });

            col.Item().PaddingTop(32);

            // Signatures Section
            col.Item().Row(r =>
            {
                // Class Teacher
                r.RelativeItem().Column(c =>
                {
                    c.Item().LineHorizontal(0.75f).LineColor(Colors.Grey.Darken3);
                    c.Item().PaddingTop(4).AlignCenter().Text("Class Teacher")
                        .FontSize(9).SemiBold().FontColor("#111827");
                });

                r.ConstantItem(60);

                // Exam Controller
                r.RelativeItem().Column(c =>
                {
                    c.Item().LineHorizontal(0.75f).LineColor(Colors.Grey.Darken3);
                    c.Item().PaddingTop(4).AlignCenter().Text("Exam Controller")
                        .FontSize(9).SemiBold().FontColor("#111827");
                });

                r.ConstantItem(60);

                // Principal
                r.RelativeItem().Column(c =>
                {
                    c.Item().LineHorizontal(0.75f).LineColor(Colors.Grey.Darken3);
                    c.Item().PaddingTop(4).AlignCenter().Text("Principal")
                        .FontSize(9).SemiBold().FontColor("#111827");
                });
            });
        });
    }

    private static byte[]? TryLoadLogoBytes()
    {
        string[] possiblePaths = {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "assets", "Edugates_Logo1.png"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Edugates_Logo1.png"),
            Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "src", "assets", "Edugates_Logo1.png"),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "ClientApp", "src", "assets", "Edugates_Logo1.png"),
            @"D:\Company\Eninas\Projects\Noman\SchoolManagementSystem\ClientApp\src\assets\Edugates_Logo1.png"
        };

        var foundPath = possiblePaths.FirstOrDefault(File.Exists);
        if (foundPath != null)
        {
            try
            {
                return File.ReadAllBytes(foundPath);
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    private static byte[]? TryLoadPhotoBytes(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath)) return null;

        string cleanPath = imagePath.TrimStart('/', '\\').Replace("/", "\\");

        string[] possiblePaths = {
            imagePath,
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", cleanPath),
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cleanPath),
            Path.Combine(Directory.GetCurrentDirectory(), cleanPath),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cleanPath),
            Path.Combine(@"D:\Company\Eninas\Projects\Noman\SchoolManagementSystem\SchoolManagementSystem.API\wwwroot", cleanPath)
        };

        var found = possiblePaths.FirstOrDefault(File.Exists);
        if (found != null)
        {
            try
            {
                return File.ReadAllBytes(found);
            }
            catch
            {
                return null;
            }
        }
        return null;
    }
}

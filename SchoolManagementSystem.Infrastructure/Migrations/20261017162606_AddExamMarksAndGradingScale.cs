using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamMarksAndGradingScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_sch_ExamComponents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TermName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxMarks = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_ExamComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_ExamComponents_tb_sch_AcademicSessions_AcademicSessionId",
                        column: x => x.AcademicSessionId,
                        principalTable: "tb_sch_AcademicSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_GradingScales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScaleName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_GradingScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_GradingScaleDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradingScaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinMark = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MaxMark = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    LetterGrade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GradePoint = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_GradingScaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_GradingScaleDetails_tb_sch_GradingScales_GradingScaleId",
                        column: x => x.GradingScaleId,
                        principalTable: "tb_sch_GradingScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_ClassSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_ClassSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_ClassSubjects_tb_sch_AcademicClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "tb_sch_AcademicClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_ClassSubjects_tb_sch_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "tb_sch_Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_StudentExamMarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarksObtained = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IsAbsent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_StudentExamMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentExamMarks_tb_sch_AcademicClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "tb_sch_AcademicClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentExamMarks_tb_sch_AcademicSessions_AcademicSessionId",
                        column: x => x.AcademicSessionId,
                        principalTable: "tb_sch_AcademicSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentExamMarks_tb_sch_ExamComponents_ExamComponentId",
                        column: x => x.ExamComponentId,
                        principalTable: "tb_sch_ExamComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentExamMarks_tb_sch_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "tb_sch_Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentExamMarks_tb_sch_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "tb_sch_Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentExamMarks_tb_sl_StudentInfo_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_sl_StudentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_ClassSubjects_ClassId",
                table: "tb_sch_ClassSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_ClassSubjects_SubjectId",
                table: "tb_sch_ClassSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_ExamComponents_AcademicSessionId",
                table: "tb_sch_ExamComponents",
                column: "AcademicSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_GradingScaleDetails_GradingScaleId",
                table: "tb_sch_GradingScaleDetails",
                column: "GradingScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentExamMarks_AcademicSessionId_StudentId_SubjectId_ExamComponentId_IsDeleted",
                table: "tb_sch_StudentExamMarks",
                columns: new[] { "AcademicSessionId", "StudentId", "SubjectId", "ExamComponentId", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentExamMarks_ClassId",
                table: "tb_sch_StudentExamMarks",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentExamMarks_ExamComponentId",
                table: "tb_sch_StudentExamMarks",
                column: "ExamComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentExamMarks_SectionId",
                table: "tb_sch_StudentExamMarks",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentExamMarks_StudentId",
                table: "tb_sch_StudentExamMarks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentExamMarks_SubjectId",
                table: "tb_sch_StudentExamMarks",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_sch_ClassSubjects");

            migrationBuilder.DropTable(
                name: "tb_sch_GradingScaleDetails");

            migrationBuilder.DropTable(
                name: "tb_sch_StudentExamMarks");

            migrationBuilder.DropTable(
                name: "tb_sch_GradingScales");

            migrationBuilder.DropTable(
                name: "tb_sch_ExamComponents");

            migrationBuilder.DropTable(
                name: "tb_sch_Subjects");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class schoolentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_sl_StudentInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_UsersLoginHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_UserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_Sitemaps",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_RoleMenus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_sch_AcademicClasses",
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
                    ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_AcademicClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_AcademicSessions",
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
                    SessionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_AcademicSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_FeeHeads",
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
                    FeeHeadName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsMonthly = table.Column<bool>(type: "bit", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_FeeHeads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_FinancialYears",
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
                    FinYearName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinCode = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_FinancialYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Institutes",
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
                    InstituteName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Institutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Sections",
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
                    SectionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Shifts",
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
                    ShiftName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_StudentGroups",
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
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_StudentGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Students",
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
                    StudentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOBNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GuardianNID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GuardianMobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Branches",
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
                    BranchName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BranchAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HomeThemeImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_Branches_tb_sch_Institutes_InstituteId",
                        column: x => x.InstituteId,
                        principalTable: "tb_sch_Institutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_FeeTemplates",
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
                    TemplateName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_FeeTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeTemplates_tb_sch_AcademicClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "tb_sch_AcademicClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeTemplates_tb_sch_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "tb_sch_Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeTemplates_tb_sch_StudentGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "tb_sch_StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_Admissions",
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
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RollNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_Admissions_tb_sch_AcademicClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "tb_sch_AcademicClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_Admissions_tb_sch_AcademicSessions_AcademicSessionId",
                        column: x => x.AcademicSessionId,
                        principalTable: "tb_sch_AcademicSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_Admissions_tb_sch_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "tb_sch_Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_Admissions_tb_sch_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "tb_sch_Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_Admissions_tb_sch_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "tb_sch_Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_tb_sch_Admissions_tb_sch_StudentGroups_GroupId",
                    //    column: x => x.GroupId,
                    //    principalTable: "tb_sch_StudentGroups",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_Admissions_tb_sch_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_sch_Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_FeeTemplateDetails",
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
                    FeeTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_FeeTemplateDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeTemplateDetails_tb_sch_FeeHeads_FeeHeadId",
                        column: x => x.FeeHeadId,
                        principalTable: "tb_sch_FeeHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeTemplateDetails_tb_sch_FeeTemplates_FeeTemplateId",
                        column: x => x.FeeTemplateId,
                        principalTable: "tb_sch_FeeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_FeeCollections",
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
                    CollectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemoNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinancialYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_FeeCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeCollections_tb_sch_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "tb_sch_Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeCollections_tb_sch_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "tb_sch_Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeCollections_tb_sch_FinancialYears_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "tb_sch_FinancialYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeCollections_tb_sch_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_sch_Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_StudentFeeLedgers",
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
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinancialYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthNo = table.Column<int>(type: "int", nullable: false),
                    YearNo = table.Column<int>(type: "int", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollectionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MemoNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VoucherCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_StudentFeeLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentFeeLedgers_tb_sch_AcademicClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "tb_sch_AcademicClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentFeeLedgers_tb_sch_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "tb_sch_Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentFeeLedgers_tb_sch_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "tb_sch_Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentFeeLedgers_tb_sch_FinancialYears_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "tb_sch_FinancialYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_StudentFeeLedgers_tb_sch_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_sch_Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_FeeCollectionDetails",
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
                    FeeCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthNo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YearNo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_FeeCollectionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeCollectionDetails_tb_sch_FeeCollections_FeeCollectionId",
                        column: x => x.FeeCollectionId,
                        principalTable: "tb_sch_FeeCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_FeeCollectionDetails_tb_sch_FeeHeads_FeeHeadId",
                        column: x => x.FeeHeadId,
                        principalTable: "tb_sch_FeeHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_AcademicClasses_ClassName_IsDeleted",
                table: "tb_sch_AcademicClasses",
                columns: new[] { "ClassName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_AcademicSessions_SessionName_IsDeleted",
                table: "tb_sch_AcademicSessions",
                columns: new[] { "SessionName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_AcademicSessionId",
                table: "tb_sch_Admissions",
                column: "AcademicSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_BranchId_AcademicSessionId_ClassId_RollNo_IsDeleted",
                table: "tb_sch_Admissions",
                columns: new[] { "BranchId", "AcademicSessionId", "ClassId", "RollNo", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_ClassId",
                table: "tb_sch_Admissions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_GroupId",
                table: "tb_sch_Admissions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_SectionId",
                table: "tb_sch_Admissions",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_ShiftId",
                table: "tb_sch_Admissions",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_StudentId",
                table: "tb_sch_Admissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Branches_InstituteId_BranchName_IsDeleted",
                table: "tb_sch_Branches",
                columns: new[] { "InstituteId", "BranchName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollectionDetails_FeeCollectionId",
                table: "tb_sch_FeeCollectionDetails",
                column: "FeeCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollectionDetails_FeeHeadId",
                table: "tb_sch_FeeCollectionDetails",
                column: "FeeHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollections_AdmissionId",
                table: "tb_sch_FeeCollections",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollections_BranchId",
                table: "tb_sch_FeeCollections",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollections_FinancialYearId",
                table: "tb_sch_FeeCollections",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollections_MemoNo_IsDeleted",
                table: "tb_sch_FeeCollections",
                columns: new[] { "MemoNo", "IsDeleted" },
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeCollections_StudentId",
                table: "tb_sch_FeeCollections",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeHeads_FeeHeadName_IsDeleted",
                table: "tb_sch_FeeHeads",
                columns: new[] { "FeeHeadName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeTemplateDetails_FeeHeadId",
                table: "tb_sch_FeeTemplateDetails",
                column: "FeeHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeTemplateDetails_FeeTemplateId_FeeHeadId_IsDeleted",
                table: "tb_sch_FeeTemplateDetails",
                columns: new[] { "FeeTemplateId", "FeeHeadId", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeTemplates_ClassId",
                table: "tb_sch_FeeTemplates",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeTemplates_GroupId",
                table: "tb_sch_FeeTemplates",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeTemplates_ShiftId",
                table: "tb_sch_FeeTemplates",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FeeTemplates_TemplateName_ClassId_GroupId_ShiftId_IsDeleted",
                table: "tb_sch_FeeTemplates",
                columns: new[] { "TemplateName", "ClassId", "GroupId", "ShiftId", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FinancialYears_FinCode_IsDeleted",
                table: "tb_sch_FinancialYears",
                columns: new[] { "FinCode", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_FinancialYears_FinYearName_IsDeleted",
                table: "tb_sch_FinancialYears",
                columns: new[] { "FinYearName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Institutes_InstituteName_IsDeleted",
                table: "tb_sch_Institutes",
                columns: new[] { "InstituteName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Sections_SectionName_IsDeleted",
                table: "tb_sch_Sections",
                columns: new[] { "SectionName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Shifts_ShiftName_IsDeleted",
                table: "tb_sch_Shifts",
                columns: new[] { "ShiftName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentFeeLedgers_AdmissionId",
                table: "tb_sch_StudentFeeLedgers",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentFeeLedgers_BranchId",
                table: "tb_sch_StudentFeeLedgers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentFeeLedgers_ClassId",
                table: "tb_sch_StudentFeeLedgers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentFeeLedgers_FinancialYearId",
                table: "tb_sch_StudentFeeLedgers",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentFeeLedgers_StudentId_AdmissionId_FinancialYearId_MonthNo_YearNo_IsDeleted",
                table: "tb_sch_StudentFeeLedgers",
                columns: new[] { "StudentId", "AdmissionId", "FinancialYearId", "MonthNo", "YearNo", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_StudentGroups_GroupName_IsDeleted",
                table: "tb_sch_StudentGroups",
                columns: new[] { "GroupName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Students_StudentCode_IsDeleted",
                table: "tb_sch_Students",
                columns: new[] { "StudentCode", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_sch_FeeCollectionDetails");

            migrationBuilder.DropTable(
                name: "tb_sch_FeeTemplateDetails");

            migrationBuilder.DropTable(
                name: "tb_sch_StudentFeeLedgers");

            migrationBuilder.DropTable(
                name: "tb_sch_FeeCollections");

            migrationBuilder.DropTable(
                name: "tb_sch_FeeHeads");

            migrationBuilder.DropTable(
                name: "tb_sch_FeeTemplates");

            migrationBuilder.DropTable(
                name: "tb_sch_Admissions");

            migrationBuilder.DropTable(
                name: "tb_sch_FinancialYears");

            migrationBuilder.DropTable(
                name: "tb_sch_AcademicClasses");

            migrationBuilder.DropTable(
                name: "tb_sch_AcademicSessions");

            migrationBuilder.DropTable(
                name: "tb_sch_Branches");

            migrationBuilder.DropTable(
                name: "tb_sch_Sections");

            migrationBuilder.DropTable(
                name: "tb_sch_Shifts");

            migrationBuilder.DropTable(
                name: "tb_sch_StudentGroups");

            migrationBuilder.DropTable(
                name: "tb_sch_Students");

            migrationBuilder.DropTable(
                name: "tb_sch_Institutes");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_sl_StudentInfo");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_UsersLoginHistory");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_Users");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_UserRoles");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_Tenants");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_Sitemaps");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_RoleMenus");
        }
    }
}

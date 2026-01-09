using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studentinfoentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_sl_StudentInfo",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthCertificateNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationForClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastClassAttendedResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PermanentAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StudentPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuardianInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalGuardianInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sl_StudentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_sl_GuardianInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FatherAcademicQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherOccupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherNationalIdNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FatherMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherTelephoneOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherTelephoneResidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MotherAcademicQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherOccupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherNationalIdNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MotherMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotherTelephoneOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherTelephoneResidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sl_GuardianInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sl_GuardianInfo_tb_sl_StudentInfo_StudentInfoId",
                        column: x => x.StudentInfoId,
                        principalTable: "tb_sl_StudentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_sl_LocalGuardianInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RelationToStudent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sl_LocalGuardianInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sl_LocalGuardianInfo_tb_sl_StudentInfo_StudentInfoId",
                        column: x => x.StudentInfoId,
                        principalTable: "tb_sl_StudentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_sl_GuardianInfo_StudentInfoId",
                table: "tb_sl_GuardianInfo",
                column: "StudentInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_sl_LocalGuardianInfo_StudentInfoId",
                table: "tb_sl_LocalGuardianInfo",
                column: "StudentInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_sl_StudentInfo_FullName_IsDeleted",
                table: "tb_sl_StudentInfo",
                columns: new[] { "FullName", "IsDeleted" },
                filter: "\"IsDeleted\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_sl_GuardianInfo");

            migrationBuilder.DropTable(
                name: "tb_sl_LocalGuardianInfo");

            migrationBuilder.DropTable(
                name: "tb_sl_StudentInfo");
        }
    }
}

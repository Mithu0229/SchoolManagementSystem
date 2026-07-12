using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class admisionupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicClasses_ClassId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicSessions_AcademicSessionId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Branches_BranchId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Sections_SectionId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Shifts_ShiftId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_StudentGroups_GroupId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Students_StudentId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_Admissions_BranchId_AcademicSessionId_ClassId_RollNo_IsDeleted",
                table: "tb_sch_Admissions");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_BranchId",
                table: "tb_sch_Admissions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_RollNo_IsDeleted",
                table: "tb_sch_Admissions",
                columns: new[] { "RollNo", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicClasses_ClassId",
                table: "tb_sch_Admissions",
                column: "ClassId",
                principalTable: "tb_sch_AcademicClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicSessions_AcademicSessionId",
                table: "tb_sch_Admissions",
                column: "AcademicSessionId",
                principalTable: "tb_sch_AcademicSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Branches_BranchId",
                table: "tb_sch_Admissions",
                column: "BranchId",
                principalTable: "tb_sch_Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Sections_SectionId",
                table: "tb_sch_Admissions",
                column: "SectionId",
                principalTable: "tb_sch_Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Shifts_ShiftId",
                table: "tb_sch_Admissions",
                column: "ShiftId",
                principalTable: "tb_sch_Shifts",
                principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tb_sch_Admissions_tb_sch_StudentGroups_GroupId",
            //    table: "tb_sch_Admissions",
            //    column: "GroupId",
            //    principalTable: "tb_sch_StudentGroups",
            //    principalColumn: "Id");

            // Clean up orphan records where StudentId doesn't exist in tb_sl_StudentInfo
            //migrationBuilder.Sql(@"
            //    -- Delete FeeCollectionDetails that reference orphan FeeCollections
            //    DELETE FROM tb_sch_FeeCollectionDetails
            //    WHERE FeeCollectionId IN (
            //        SELECT Id FROM tb_sch_FeeCollections
            //        WHERE AdmissionId IN (
            //            SELECT Id FROM tb_sch_Admissions
            //            WHERE StudentId NOT IN (SELECT Id FROM tb_sl_StudentInfo)
            //        )
            //    );

            //    -- Delete FeeCollections that reference orphan Admissions
            //    DELETE FROM tb_sch_FeeCollections
            //    WHERE AdmissionId IN (
            //        SELECT Id FROM tb_sch_Admissions
            //        WHERE StudentId NOT IN (SELECT Id FROM tb_sl_StudentInfo)
            //    );

            //    -- Delete BillDetails that reference orphan BillMasters
            //    DELETE FROM tb_sch_BillDetails
            //    WHERE BillMasterId IN (
            //        SELECT Id FROM tb_sch_BillMasters
            //        WHERE AdmissionId IN (
            //            SELECT Id FROM tb_sch_Admissions
            //            WHERE StudentId NOT IN (SELECT Id FROM tb_sl_StudentInfo)
            //        )
            //    );

            //    -- Delete BillMasters that reference orphan Admissions
            //    DELETE FROM tb_sch_BillMasters
            //    WHERE AdmissionId IN (
            //        SELECT Id FROM tb_sch_Admissions
            //        WHERE StudentId NOT IN (SELECT Id FROM tb_sl_StudentInfo)
            //    );

            //    -- Delete StudentFeeLedgers that reference orphan Admissions
            //    DELETE FROM tb_sch_StudentFeeLedgers
            //    WHERE AdmissionId IN (
            //        SELECT Id FROM tb_sch_Admissions
            //        WHERE StudentId NOT IN (SELECT Id FROM tb_sl_StudentInfo)
            //    );

            //    -- Finally delete orphan Admissions
            //    DELETE FROM tb_sch_Admissions
            //    WHERE StudentId NOT IN (SELECT Id FROM tb_sl_StudentInfo);
            //");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_Admissions",
                column: "StudentId",
                principalTable: "tb_sl_StudentInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicClasses_ClassId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicSessions_AcademicSessionId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Branches_BranchId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Sections_SectionId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Shifts_ShiftId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_StudentGroups_GroupId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_Admissions_BranchId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_Admissions_RollNo_IsDeleted",
                table: "tb_sch_Admissions");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_BranchId_AcademicSessionId_ClassId_RollNo_IsDeleted",
                table: "tb_sch_Admissions",
                columns: new[] { "BranchId", "AcademicSessionId", "ClassId", "RollNo", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicClasses_ClassId",
                table: "tb_sch_Admissions",
                column: "ClassId",
                principalTable: "tb_sch_AcademicClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_AcademicSessions_AcademicSessionId",
                table: "tb_sch_Admissions",
                column: "AcademicSessionId",
                principalTable: "tb_sch_AcademicSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Branches_BranchId",
                table: "tb_sch_Admissions",
                column: "BranchId",
                principalTable: "tb_sch_Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Sections_SectionId",
                table: "tb_sch_Admissions",
                column: "SectionId",
                principalTable: "tb_sch_Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Shifts_ShiftId",
                table: "tb_sch_Admissions",
                column: "ShiftId",
                principalTable: "tb_sch_Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tb_sch_Admissions_tb_sch_StudentGroups_GroupId",
            //    table: "tb_sch_Admissions",
            //    column: "GroupId",
            //    principalTable: "tb_sch_StudentGroups",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Students_StudentId",
                table: "tb_sch_Admissions",
                column: "StudentId",
                principalTable: "tb_sch_Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

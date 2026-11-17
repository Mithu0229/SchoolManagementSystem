using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstituteIdToTenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_InstituteId",
                table: "tb_sch_Branches");

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
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_gs_RoleMenus");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Teacheres",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Subjects",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Students",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_StudentGroups",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_StudentFeeLedgers",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_StudentExamMarks",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Shifts",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Sections",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Institutes",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_GradingScales",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_FinancialYears",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_FeeTemplates",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_FeeTemplateDetails",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_FeeHeads",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_FeeCollections",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_FeeCollectionDetails",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_ExamComponents",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_ClassSubjects",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_CashBooks",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Branches",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_sch_Branches_InstituteId_BranchName_IsDeleted",
                table: "tb_sch_Branches",
                newName: "IX_tb_sch_Branches_TenantId_BranchName_IsDeleted");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_BkashTransactions",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_BillMasters",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_BillDetails",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_BankBooks",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_AttendanceDevices",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_Admissions",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_AcademicSessions",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_sch_AcademicClasses",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_gs_Tenants",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "tb_gs_Sitemaps",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "InstituteId",
                table: "SMSHistories",
                newName: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_TenantId",
                table: "tb_sch_Branches",
                column: "TenantId",
                principalTable: "tb_sch_Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_TenantId",
                table: "tb_sch_Branches");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Teacheres",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Subjects",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Students",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_StudentGroups",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_StudentFeeLedgers",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_StudentExamMarks",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Shifts",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Sections",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Institutes",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_GradingScales",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_FinancialYears",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_FeeTemplates",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_FeeTemplateDetails",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_FeeHeads",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_FeeCollections",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_FeeCollectionDetails",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_ExamComponents",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_ClassSubjects",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_CashBooks",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Branches",
                newName: "InstituteId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_sch_Branches_TenantId_BranchName_IsDeleted",
                table: "tb_sch_Branches",
                newName: "IX_tb_sch_Branches_InstituteId_BranchName_IsDeleted");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_BkashTransactions",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_BillMasters",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_BillDetails",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_BankBooks",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_AttendanceDevices",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_Admissions",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_AcademicSessions",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_sch_AcademicClasses",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_gs_Tenants",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "tb_gs_Sitemaps",
                newName: "InstituteId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "SMSHistories",
                newName: "InstituteId");

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
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_gs_RoleMenus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_InstituteId",
                table: "tb_sch_Branches",
                column: "InstituteId",
                principalTable: "tb_sch_Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class branchupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_TenantId",
                table: "tb_sch_Branches");

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "tb_sch_Branches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Branches_InstituteId",
                table: "tb_sch_Branches",
                column: "InstituteId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_InstituteId",
                table: "tb_sch_Branches",
                column: "InstituteId",
                principalTable: "tb_sch_Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_InstituteId",
                table: "tb_sch_Branches");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_Branches_InstituteId",
                table: "tb_sch_Branches");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "tb_sch_Branches");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Branches_tb_sch_Institutes_TenantId",
                table: "tb_sch_Branches",
                column: "TenantId",
                principalTable: "tb_sch_Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

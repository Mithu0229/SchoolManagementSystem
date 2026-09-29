using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cashbookandbankbookupdatenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "tb_sch_CashBooks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "tb_sch_BankBooks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_CashBooks_StudentId",
                table: "tb_sch_CashBooks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_BankBooks_StudentId",
                table: "tb_sch_BankBooks",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_BankBooks_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_BankBooks",
                column: "StudentId",
                principalTable: "tb_sl_StudentInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_CashBooks_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_CashBooks",
                column: "StudentId",
                principalTable: "tb_sl_StudentInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_BankBooks_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_BankBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_CashBooks_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_CashBooks");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_CashBooks_StudentId",
                table: "tb_sch_CashBooks");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_BankBooks_StudentId",
                table: "tb_sch_BankBooks");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "tb_sch_CashBooks");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "tb_sch_BankBooks");
        }
    }
}

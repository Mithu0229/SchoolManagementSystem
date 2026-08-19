using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatebill1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromNumber",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.AddColumn<string>(
                name: "BillMonth",
                table: "tb_sch_BkashTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerNo",
                table: "tb_sch_BkashTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayTime",
                table: "tb_sch_BkashTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrxId",
                table: "tb_sch_BkashTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "tb_sch_BkashTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserMobileNumber",
                table: "tb_sch_BkashTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillMonth",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.DropColumn(
                name: "CustomerNo",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.DropColumn(
                name: "PayTime",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.DropColumn(
                name: "TrxId",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.DropColumn(
                name: "UserMobileNumber",
                table: "tb_sch_BkashTransactions");

            migrationBuilder.AddColumn<string>(
                name: "FromNumber",
                table: "tb_sch_BkashTransactions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mulitbillupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VoucherNo",
                table: "tb_sch_BillMasters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "CollectionAmount",
                table: "tb_sch_BillMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "tb_sch_BillMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);



            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "tb_sch_BillMasters",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionAmount",
                table: "tb_sch_BillMasters");

            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "tb_sch_BillMasters");



            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "tb_sch_BillMasters");

            migrationBuilder.AlterColumn<string>(
                name: "VoucherNo",
                table: "tb_sch_BillMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

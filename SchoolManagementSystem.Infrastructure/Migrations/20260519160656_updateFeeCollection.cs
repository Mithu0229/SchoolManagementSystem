using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFeeCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthNo",
                table: "tb_sch_FeeCollectionDetails");

            migrationBuilder.DropColumn(
                name: "YearNo",
                table: "tb_sch_FeeCollectionDetails");

            migrationBuilder.AddColumn<int>(
                name: "MonthNo",
                table: "tb_sch_FeeCollectionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearNo",
                table: "tb_sch_FeeCollectionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthNo",
                table: "tb_sch_FeeCollectionDetails");

            migrationBuilder.DropColumn(
                name: "YearNo",
                table: "tb_sch_FeeCollectionDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "MonthNo",
                table: "tb_sch_FeeCollectionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "YearNo",
                table: "tb_sch_FeeCollectionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);
        }
    }
}

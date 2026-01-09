using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class studententityupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuardianInfoId",
                table: "tb_sl_StudentInfo");

            migrationBuilder.DropColumn(
                name: "LocalGuardianInfoId",
                table: "tb_sl_StudentInfo");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "tb_sl_StudentInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "tb_sl_StudentInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "GuardianInfoId",
                table: "tb_sl_StudentInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalGuardianInfoId",
                table: "tb_sl_StudentInfo",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

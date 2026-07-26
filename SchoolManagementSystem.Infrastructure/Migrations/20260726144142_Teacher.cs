using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Teacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "tb_sch_Admissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_sch_Teacheres",
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
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_Teacheres", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_Admissions_TeacherId",
                table: "tb_sch_Admissions",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Teacheres_TeacherId",
                table: "tb_sch_Admissions",
                column: "TeacherId",
                principalTable: "tb_sch_Teacheres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_Admissions_tb_sch_Teacheres_TeacherId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropTable(
                name: "tb_sch_Teacheres");

            migrationBuilder.DropIndex(
                name: "IX_tb_sch_Admissions_TeacherId",
                table: "tb_sch_Admissions");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "tb_sch_Admissions");
        }
    }
}

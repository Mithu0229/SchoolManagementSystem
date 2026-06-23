using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class admisionupdateall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_FeeCollections_tb_sch_Students_StudentId",
                table: "tb_sch_FeeCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_StudentFeeLedgers_tb_sch_Students_StudentId",
                table: "tb_sch_StudentFeeLedgers");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_FeeCollections_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_FeeCollections",
                column: "StudentId",
                principalTable: "tb_sl_StudentInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_StudentFeeLedgers_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_StudentFeeLedgers",
                column: "StudentId",
                principalTable: "tb_sl_StudentInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_FeeCollections_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_FeeCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_StudentFeeLedgers_tb_sl_StudentInfo_StudentId",
                table: "tb_sch_StudentFeeLedgers");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_FeeCollections_tb_sch_Students_StudentId",
                table: "tb_sch_FeeCollections",
                column: "StudentId",
                principalTable: "tb_sch_Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_StudentFeeLedgers_tb_sch_Students_StudentId",
                table: "tb_sch_StudentFeeLedgers",
                column: "StudentId",
                principalTable: "tb_sch_Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

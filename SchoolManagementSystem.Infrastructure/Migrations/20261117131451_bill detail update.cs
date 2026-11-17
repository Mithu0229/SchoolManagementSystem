using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class billdetailupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_BillDetails_tb_sch_FeeTemplateDetails_FeeTemplateDetailId",
                table: "tb_sch_BillDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_BillDetails_tb_sch_FeeTemplateDetails_FeeTemplateDetailId",
                table: "tb_sch_BillDetails",
                column: "FeeTemplateDetailId",
                principalTable: "tb_sch_FeeTemplateDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_sch_BillDetails_tb_sch_FeeTemplateDetails_FeeTemplateDetailId",
                table: "tb_sch_BillDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_sch_BillDetails_tb_sch_FeeTemplateDetails_FeeTemplateDetailId",
                table: "tb_sch_BillDetails",
                column: "FeeTemplateDetailId",
                principalTable: "tb_sch_FeeTemplateDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

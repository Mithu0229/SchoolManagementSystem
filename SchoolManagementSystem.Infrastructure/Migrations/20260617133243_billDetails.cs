using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class billDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_sch_BillMasters",
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
                    AdmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillMonth = table.Column<int>(type: "int", nullable: false),
                    BillYear = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_BillMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_BillMasters_tb_sch_Admissions_AdmissionId",
                        column: x => x.AdmissionId,
                        principalTable: "tb_sch_Admissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_sch_BillDetails",
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
                    BillMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeTemplateDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sch_BillDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_sch_BillDetails_tb_sch_BillMasters_BillMasterId",
                        column: x => x.BillMasterId,
                        principalTable: "tb_sch_BillMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_BillDetails_tb_sch_FeeHeads_FeeHeadId",
                        column: x => x.FeeHeadId,
                        principalTable: "tb_sch_FeeHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_sch_BillDetails_tb_sch_FeeTemplateDetails_FeeTemplateDetailId",
                        column: x => x.FeeTemplateDetailId,
                        principalTable: "tb_sch_FeeTemplateDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_BillDetails_BillMasterId",
                table: "tb_sch_BillDetails",
                column: "BillMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_BillDetails_FeeHeadId",
                table: "tb_sch_BillDetails",
                column: "FeeHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_BillDetails_FeeTemplateDetailId",
                table: "tb_sch_BillDetails",
                column: "FeeTemplateDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sch_BillMasters_AdmissionId_BillMonth_BillYear_IsDeleted",
                table: "tb_sch_BillMasters",
                columns: new[] { "AdmissionId", "BillMonth", "BillYear", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_sch_BillDetails");

            migrationBuilder.DropTable(
                name: "tb_sch_BillMasters");
        }
    }
}

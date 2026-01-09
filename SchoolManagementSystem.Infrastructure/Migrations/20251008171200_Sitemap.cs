using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sitemap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_gs_Users_Email_IsDeleted",
                table: "tb_gs_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "tb_gs_Roles");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "tb_gs_Users",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 107);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "tb_gs_Roles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 106);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tb_gs_Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "tb_gs_Roles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .Annotation("Relational:ColumnOrder", 104);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 103);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteRequestedOn",
                table: "tb_gs_Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 101);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "tb_gs_Roles",
                type: "datetime2",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 102);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tb_gs_Roles",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 90);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tb_gs_Roles",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 100);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedById",
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 105);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "tb_gs_Roles",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoleType",
                table: "tb_gs_Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "tb_gs_Roles",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 107);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_gs_Roles",
                table: "tb_gs_Roles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "tb_gs_Sitemaps",
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
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    FavIcon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    SortingOrder = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsFeature = table.Column<bool>(type: "bit", nullable: false),
                    IsSidebarmenu = table.Column<bool>(type: "bit", nullable: false),
                    MenuType = table.Column<int>(type: "int", nullable: false),
                    MenuAccessType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gs_Sitemaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_gs_Sitemaps_tb_gs_Sitemaps_ParentId",
                        column: x => x.ParentId,
                        principalTable: "tb_gs_Sitemaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_gs_Tenants",
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
                    TenantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BinNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gs_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_gs_UserRoles",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gs_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_gs_UserRoles_tb_gs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_gs_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_gs_UserRoles_tb_gs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_gs_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_gs_RoleMenus",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SitemapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CanView = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CanAdd = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CanPreview = table.Column<bool>(type: "bit", nullable: false),
                    CanExport = table.Column<bool>(type: "bit", nullable: false),
                    CanPrint = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gs_RoleMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_gs_RoleMenus_tb_gs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_gs_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_gs_RoleMenus_tb_gs_Sitemaps_SitemapId",
                        column: x => x.SitemapId,
                        principalTable: "tb_gs_Sitemaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Users_Email_IsDeleted",
                table: "tb_gs_Users",
                columns: new[] { "Email", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Users_TenantId",
                table: "tb_gs_Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Roles_TenantId_RoleName_IsDeleted",
                table: "tb_gs_Roles",
                columns: new[] { "TenantId", "RoleName", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_RoleMenus_RoleId",
                table: "tb_gs_RoleMenus",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_RoleMenus_SitemapId_RoleId_TenantId_IsDeleted",
                table: "tb_gs_RoleMenus",
                columns: new[] { "SitemapId", "RoleId", "TenantId", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Sitemaps_Name_PageUrl",
                table: "tb_gs_Sitemaps",
                columns: new[] { "Name", "PageUrl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Sitemaps_ParentId",
                table: "tb_gs_Sitemaps",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Tenants_TenantEmail_IsDeleted",
                table: "tb_gs_Tenants",
                columns: new[] { "TenantEmail", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_UserRoles_RoleId",
                table: "tb_gs_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_UserRoles_UserId_RoleId_TenantId_IsDeleted",
                table: "tb_gs_UserRoles",
                columns: new[] { "UserId", "RoleId", "TenantId", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_gs_Roles_tb_gs_Tenants_TenantId",
                table: "tb_gs_Roles",
                column: "TenantId",
                principalTable: "tb_gs_Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_gs_Users_tb_gs_Tenants_TenantId",
                table: "tb_gs_Users",
                column: "TenantId",
                principalTable: "tb_gs_Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_gs_Roles_tb_gs_Tenants_TenantId",
                table: "tb_gs_Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_gs_Users_tb_gs_Tenants_TenantId",
                table: "tb_gs_Users");

            migrationBuilder.DropTable(
                name: "tb_gs_RoleMenus");

            migrationBuilder.DropTable(
                name: "tb_gs_Tenants");

            migrationBuilder.DropTable(
                name: "tb_gs_UserRoles");

            migrationBuilder.DropTable(
                name: "tb_gs_Sitemaps");

            migrationBuilder.DropIndex(
                name: "IX_tb_gs_Users_Email_IsDeleted",
                table: "tb_gs_Users");

            migrationBuilder.DropIndex(
                name: "IX_tb_gs_Users_TenantId",
                table: "tb_gs_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_gs_Roles",
                table: "tb_gs_Roles");

            migrationBuilder.DropIndex(
                name: "IX_tb_gs_Roles_TenantId_RoleName_IsDeleted",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "tb_gs_Users");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "DeleteRequestedOn",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "RoleType",
                table: "tb_gs_Roles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "tb_gs_Roles");

            migrationBuilder.RenameTable(
                name: "tb_gs_Roles",
                newName: "Roles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 106);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()")
                .OldAnnotation("Relational:ColumnOrder", 104);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gs_Users_Email_IsDeleted",
                table: "tb_gs_Users",
                columns: new[] { "Email", "IsDeleted" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }
    }
}

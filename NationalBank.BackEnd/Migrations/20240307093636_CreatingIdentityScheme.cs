using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NationalBank.BackEnd.Migrations
{
    public partial class CreatingIdentityScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationRegister",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Application_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_fathername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_mothername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Application_gender = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Application_qualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_MartialStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Application_Hobbies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_Registerdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Application_IsAcceptedTermsandConditions = table.Column<bool>(type: "bit", nullable: false),
                    Application_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Application_DistrictId = table.Column<long>(type: "bigint", nullable: false),
                    Application_StateId = table.Column<long>(type: "bigint", nullable: false),
                    Application_CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Application_IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    Application_ApprovedBy = table.Column<long>(type: "bigint", nullable: true),
                    Application_ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Application_ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Application_Status = table.Column<bool>(type: "bit", nullable: false),
                    Rowstate = table.Column<byte>(type: "tinyint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rowstate = table.Column<byte>(type: "tinyint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationDocumentUploads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Rowstate = table.Column<byte>(type: "tinyint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDocumentUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDocumentUploads_ApplicationRegister",
                        column: x => x.ApplicationId,
                        principalTable: "ApplicationRegister",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationDocumentUploads_DocumentTypes",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocumentUploads_ApplicationId",
                table: "ApplicationDocumentUploads",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDocumentUploads_DocumentTypeId",
                table: "ApplicationDocumentUploads",
                column: "DocumentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationDocumentUploads");

            migrationBuilder.DropTable(
                name: "ApplicationRegister");

            migrationBuilder.DropTable(
                name: "DocumentTypes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Migrations
{
    public partial class Add_Tokens_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a30b6097-094a-4cd3-a635-b776962e889b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("be34abaa-9333-420a-ab9a-148ab5f4a7df"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dd888c50-7f42-4120-bc26-c4a32e2cd619"));

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Issued = table.Column<DateTimeOffset>(nullable: false),
                    Expires = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("766ffb44-2a23-4c3e-b526-ccda53f138f5"), "User" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("c5cfc569-ca50-4f7a-8a94-b9b2908a15fa"), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("a4ac1a3b-a96f-4550-a254-cfb894e1a713"), "System" });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId",
                table: "Tokens",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("766ffb44-2a23-4c3e-b526-ccda53f138f5"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a4ac1a3b-a96f-4550-a254-cfb894e1a713"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c5cfc569-ca50-4f7a-8a94-b9b2908a15fa"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("be34abaa-9333-420a-ab9a-148ab5f4a7df"), "Account" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("a30b6097-094a-4cd3-a635-b776962e889b"), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("dd888c50-7f42-4120-bc26-c4a32e2cd619"), "System" });
        }
    }
}

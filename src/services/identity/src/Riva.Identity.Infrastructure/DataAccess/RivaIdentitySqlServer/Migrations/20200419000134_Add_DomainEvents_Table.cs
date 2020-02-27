using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Migrations
{
    public partial class Add_DomainEvents_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastLogin",
                table: "Accounts",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.CreateTable(
                name: "DomainEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AggregateId = table.Column<Guid>(nullable: false),
                    CorrelationId = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    FullyQualifiedEventTypeName = table.Column<string>(nullable: false),
                    EventData = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("2740765f-db34-40ca-b620-3db34e527e5b"), "User" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("0f7fc90e-1d6e-4c56-9ef4-24a79cbfd3fb"), "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("fb32791c-b457-4a17-b31e-671106ff1506"), "System" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainEvents");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0f7fc90e-1d6e-4c56-9ef4-24a79cbfd3fb"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2740765f-db34-40ca-b620-3db34e527e5b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("fb32791c-b457-4a17-b31e-671106ff1506"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastLogin",
                table: "Accounts",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

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
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Migrations
{
    public partial class Add_States_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PolishName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("23cbd1a9-78b5-4b7f-82f8-510e792d6022"), "Dolnoslaskie", "Dolnośląskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("0b2d0ba0-0365-4d6a-a2bf-f05a9fd08871"), "Kujawsko-pomorskie", "Kujawsko-pomorskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("9d2640f7-266e-4274-9f42-a9cd14aa41e1"), "Lubelskie", "Lubelskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("c2c3864c-7d24-4f26-a286-9e859352e231"), "Lubuskie", "Lubuskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("b2d6b891-65a3-4b8e-8a15-f80b335d4e1a"), "Lodzkie", "Łódzkie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("f5168cc9-f98d-4a4e-a4d9-c3fc120d1987"), "Malopolskie", "Małopolskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("008e4495-51ad-4c2c-ab97-6f93f4aaa42e"), "Mazowieckie", "Mazowieckie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("c90a386f-1866-4792-8bb6-b2f4b244126c"), "Opolskie", "Opolskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("59fd3598-a69e-47aa-8f0b-09714f605267"), "Podkarpackie", "Podkarpackie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("ac1a4a8e-fd7a-4b00-a3f0-9a6a9d071497"), "Podlaskie", "Podlaskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("6712c6ae-b8cf-4447-9d63-37f980d5af83"), "Pomorskie", "Pomorskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("c44814d9-357b-4bc0-b4a2-25d056e1b448"), "Slaskie", "Śląskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("d22ea8e8-0f4d-4f1d-b04f-15ae2e58cb35"), "Swietokrzyskie", "Świętokrzyskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("dbc5ecba-6560-40cd-8639-1ca31a606b01"), "Warminsko-mazurskie", "Warmińsko-mazurskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("b6927901-a3e7-44be-bcfc-0f01e669dc7f"), "Wielkopolskie", "Wielkopolskie" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name", "PolishName" },
                values: new object[] { new Guid("baadea02-7671-454e-8cc0-dc5b8e71af5d"), "Zachodniopomorskie", "Zachodniopomorskie" });

            migrationBuilder.CreateIndex(
                name: "IX_States_Name",
                table: "States",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_PolishName",
                table: "States",
                column: "PolishName",
                unique: true,
                filter: "[PolishName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "States");
        }
    }
}

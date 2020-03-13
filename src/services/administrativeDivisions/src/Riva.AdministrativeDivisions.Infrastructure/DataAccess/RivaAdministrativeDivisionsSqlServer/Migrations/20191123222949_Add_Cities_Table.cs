using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Migrations
{
    public partial class Add_Cities_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PolishName = table.Column<string>(maxLength: 256, nullable: false),
                    StateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "PolishName", "StateId" },
                values: new object[] { new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Wroclaw", "Wrocław", new Guid("23cbd1a9-78b5-4b7f-82f8-510e792d6022") });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name_StateId",
                table: "Cities",
                columns: new[] { "Name", "StateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_PolishName_StateId",
                table: "Cities",
                columns: new[] { "PolishName", "StateId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Migrations
{
    public partial class Add_Cities_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                column: "Id",
                value: new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}

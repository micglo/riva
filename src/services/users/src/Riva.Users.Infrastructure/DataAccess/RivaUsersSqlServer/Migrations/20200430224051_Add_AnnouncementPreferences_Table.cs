using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Migrations
{
    public partial class Add_AnnouncementPreferences_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnouncementPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CityId = table.Column<Guid>(nullable: false),
                    AnnouncementPreferenceType = table.Column<string>(nullable: false),
                    RoomType = table.Column<string>(nullable: true),
                    PriceMin = table.Column<decimal>(type: "decimal(38, 2)", nullable: true),
                    PriceMax = table.Column<decimal>(type: "decimal(38, 2)", nullable: true),
                    RoomNumbersMin = table.Column<int>(nullable: true),
                    RoomNumbersMax = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementPreferences_UserId",
                table: "AnnouncementPreferences",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementPreferences");
        }
    }
}

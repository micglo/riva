using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Migrations
{
    public partial class Add_AnnouncementPreferenceCityDistricts_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnouncementPreferenceCityDistricts",
                columns: table => new
                {
                    AnnouncementPreferenceId = table.Column<Guid>(nullable: false),
                    CityDistrictId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementPreferenceCityDistricts", x => new { x.AnnouncementPreferenceId, x.CityDistrictId });
                    table.ForeignKey(
                        name: "FK_AnnouncementPreferenceCityDistricts_AnnouncementPreferences_AnnouncementPreferenceId",
                        column: x => x.AnnouncementPreferenceId,
                        principalTable: "AnnouncementPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementPreferenceCityDistricts_CityDistricts_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementPreferenceCityDistricts_CityDistrictId",
                table: "AnnouncementPreferenceCityDistricts",
                column: "CityDistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementPreferenceCityDistricts");
        }
    }
}

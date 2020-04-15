using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Migrations
{
    public partial class Add_Users_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    Picture = table.Column<string>(maxLength: 256, nullable: true),
                    ServiceActive = table.Column<bool>(nullable: false),
                    AnnouncementPreferenceLimit = table.Column<int>(nullable: false),
                    AnnouncementSendingFrequency = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

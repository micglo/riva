using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Migrations
{
    public partial class Add_DomainEvents_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainEvents");
        }
    }
}

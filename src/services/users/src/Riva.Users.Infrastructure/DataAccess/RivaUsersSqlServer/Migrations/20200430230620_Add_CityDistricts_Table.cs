using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Migrations
{
    public partial class Add_CityDistricts_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityDistricts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityDistricts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CityDistricts",
                columns: new[] { "Id", "CityId" },
                values: new object[,]
                {
                    { new Guid("882cc824-1d0b-4acd-89d0-c846b98ab946"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("ddcc1be6-395e-43aa-ad72-a150b868f7f3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("86c0e69c-1259-4245-b94f-9c33d12aa490"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("a63d5148-471b-422c-b016-0317c9689cd3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("2fe676de-ebef-4e28-a012-61da79da6947"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("29b9a280-a2a5-4436-810c-78df10740360"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("ffe59d30-48ca-4534-a798-5406de30775d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("3b4ea3b0-cd9a-43a9-8af5-9636afaf82a1"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("7930362c-3810-43d6-a8a6-61f7bee04846"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("abe94cf7-c965-4223-b7df-be2b72f0fe28"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("1c6699c2-d11f-41a1-bc25-69ec07279176"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("90c8dcff-3eda-4776-ba1e-fc505bcb89fd"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("33a46874-6ba2-46ab-a772-74f750a83f77"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("2a5a62cf-051f-4270-a35b-8ec4a106c522"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("9446441a-79d3-4b14-949d-384ad704165d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("1d087abe-fee0-46b9-9212-c72d4bad5c0b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("ceb9a82f-0141-49ad-9927-8aeae16da244"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("d83841fa-27fb-4f89-aab2-c40088f75980"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("6d9ed1ea-dda0-46c7-9649-4f18f682efda"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("1c2ef079-52ef-4568-a06e-f882dcf28c23"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("1cd70b69-127f-4deb-a5cb-186c89a73861"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("918ce4d5-989f-4458-acc2-e00509b23a20"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("25fb993b-700f-45be-8699-c09881f7a76c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("ac5d8893-9370-4546-bbb5-51edc01a442c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("e5e77c3d-bb85-4c7a-868e-43e5d6f623cc"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("c75ff32f-6ee3-4347-86b4-1aa0a96651a7"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("f6c8b915-38cd-44d8-878d-115cc440c5d3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("4a7cfa64-3c42-46d7-b694-f6b8eccb5ab6"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("a45a6df5-4695-4db2-a362-c0477eab1c8b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("96adb77d-8505-46fc-9515-41fee1f06a6a"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("aa5f1748-d858-45c6-b1bd-fd68d98c8b38"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("66f68e3a-50cf-49cd-8191-e1fc627ea2e4"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("e8858102-054a-4779-9760-7380ecf08df3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("7dea11fb-65ed-4b98-a06c-cfd01917b655"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("853f916a-3fbd-4baf-8a09-f29eee498966"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("7f7e9487-cb8d-489b-8a58-827756622895"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("a25aa6e2-70dd-413f-9b00-949042b14f97"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("69c05563-4af9-4552-b43d-b742dc33a91f"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("88dd1ffd-00f4-469b-ae94-745e2a336ddb"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("0a77f641-5227-4f4e-8f89-91d431218daa"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("59a2d9a7-e6dd-4b8e-b1e7-47e88f18e0dc"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("1019e1be-57d7-4ca6-9b38-f5da790eaa33"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") },
                    { new Guid("8932a433-c5b2-4c21-80ae-2f9e018f4a3e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_CityId",
                table: "CityDistricts",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityDistricts");
        }
    }
}

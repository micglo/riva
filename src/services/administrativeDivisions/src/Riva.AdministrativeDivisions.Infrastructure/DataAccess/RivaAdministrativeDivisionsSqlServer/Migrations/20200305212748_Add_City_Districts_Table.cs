using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Migrations
{
    public partial class Add_City_Districts_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityDistricts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    PolishName = table.Column<string>(maxLength: 256, nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "CityDistrictNameVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(maxLength: 256, nullable: false),
                    CityDistrictId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDistrictNameVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityDistrictNameVariants_CityDistricts_CityDistrictId",
                        column: x => x.CityDistrictId,
                        principalTable: "CityDistricts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CityDistricts",
                columns: new[] { "Id", "CityId", "Name", "ParentId", "PolishName" },
                values: new object[,]
                {
                    { new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Fabryczna", null, "Fabryczna" },
                    { new Guid("b2669d90-3214-49f7-b17b-f974e3b8f22c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Ksieze-Wielkie", new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Księże Wielkie" },
                    { new Guid("2615becf-bbc6-4ad6-b37b-68f1d3b9265d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Ksieze-Male", new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Księże Małe" },
                    { new Guid("eff858bd-c899-4a2c-80ad-f6800ea0de14"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Partynice", new Guid("ac5d8893-9370-4546-bbb5-51edc01a442c"), "Partynice" },
                    { new Guid("4d965d02-a05a-4c4b-a8c3-e9504b518185"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Glinianki", new Guid("88dd1ffd-00f4-469b-ae94-745e2a336ddb"), "Glinianki" },
                    { new Guid("2b25ded6-a25f-441d-b8b8-53d7b7a8e255"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Nowa-Karczma", new Guid("e8858102-054a-4779-9760-7380ecf08df3"), "Nowa Karczma" },
                    { new Guid("3a91b41d-5cf7-4480-bb5c-ed9179d7c00b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Janowek", new Guid("e8858102-054a-4779-9760-7380ecf08df3"), "Janówek" },
                    { new Guid("c91dc887-4deb-4274-abe3-be0baed37a33"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Popowice", new Guid("1019e1be-57d7-4ca6-9b38-f5da790eaa33"), "Popowice" },
                    { new Guid("7ff08f6a-cfdb-40d2-b57f-0c96288b8d7a"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Kozanow", new Guid("1019e1be-57d7-4ca6-9b38-f5da790eaa33"), "Kozanów" },
                    { new Guid("4f2a852b-2c4c-44ca-b5ad-646221313c4c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Pilczyce", new Guid("1019e1be-57d7-4ca6-9b38-f5da790eaa33"), "Pilczyce" },
                    { new Guid("a4ce962e-5246-4952-80ae-0087f1266700"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Gajowa", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Gajowa" },
                    { new Guid("cf8556b4-eaef-4eed-ba5e-98ce8eb3822b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Swiatniki", new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Świątniki" },
                    { new Guid("93dd35eb-f541-4b84-9405-5bbe23c94f2d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zar", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Żar" },
                    { new Guid("ab96fd01-5a2d-4f2a-9b89-626b092ae4cb"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Mokra", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Mokra" },
                    { new Guid("4e7c7585-be5a-4d4d-a87b-0ec49f32fd4c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Ratyn", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Ratyń" },
                    { new Guid("caceb67b-1738-459d-a8f7-a5482604bfdd"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Marszowice", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Marszowice" },
                    { new Guid("6f8f3d9e-dcec-42f0-bec1-09a825cc3930"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zlotniki", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Złotniki" },
                    { new Guid("852cb711-daae-4cca-b65b-62283725e5da"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Stablowice", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Stabłowice" },
                    { new Guid("e660b554-5262-41bc-bf85-394fd6e281d0"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Osiniec", new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9"), "Osiniec" },
                    { new Guid("30fcd3fe-1733-49f6-b0ff-b58586454c86"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Strachowice", new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9"), "Strachowice" },
                    { new Guid("a1c44a64-da37-4dae-b099-0f952f259a56"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Jarnoltow", new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9"), "Jarnołtów" },
                    { new Guid("9b465ded-eed0-4ea4-9bf4-ebf3a642dd4b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Jerzmanowo", new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9"), "Jerzmanowo" },
                    { new Guid("914214ae-cd23-4a67-8af6-0cf580813b6c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Grabiszynek", new Guid("c75ff32f-6ee3-4347-86b4-1aa0a96651a7"), "Grabiszynek" },
                    { new Guid("7dec37f3-d52d-477d-a292-ee1f617a6b3e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Pustki", new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Pustki" },
                    { new Guid("ba687519-0138-40e2-ac65-77d297c4955d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Opatowice", new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Opatowice" },
                    { new Guid("9cee8e4c-89bf-4cbc-8476-b130dabae470"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Bierdzany", new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Bierdzany" },
                    { new Guid("d8abcb58-1ee0-45dc-bc44-633660d09d75"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Nowy-Dom", new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Nowy Dom" },
                    { new Guid("c566fd0d-0873-4f6f-8b0a-070ea521dc93"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Szczytniki", new Guid("8932a433-c5b2-4c21-80ae-2f9e018f4a3e"), "Szczytniki" },
                    { new Guid("c4de6e53-8b6e-45e1-a587-7fc5ae2ef8ac"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Sepolno", new Guid("d83841fa-27fb-4f89-aab2-c40088f75980"), "Sępolno" },
                    { new Guid("69838cb4-81f9-4e05-a50d-d224a039ce89"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Dabie", new Guid("d83841fa-27fb-4f89-aab2-c40088f75980"), "Dąbie" },
                    { new Guid("b2244039-7b02-4221-bfa5-e58880238cdb"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Biskupin", new Guid("d83841fa-27fb-4f89-aab2-c40088f75980"), "Biskupin" },
                    { new Guid("830582c7-1783-4610-a6c1-804e6af3c2f8"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Bartoszowice", new Guid("d83841fa-27fb-4f89-aab2-c40088f75980"), "Bartoszowice" },
                    { new Guid("74e43ed2-2357-4895-afbd-836cd2b9cc5e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Popiele", new Guid("33a46874-6ba2-46ab-a772-74f750a83f77"), "Popiele" },
                    { new Guid("7ca1f8fc-fc80-4758-a884-c231d2d74934"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Wojnow", new Guid("33a46874-6ba2-46ab-a772-74f750a83f77"), "Wojnów" },
                    { new Guid("6b2513f1-c8c2-4bf3-b5de-7dbb43a8ab81"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Swojczyce", new Guid("33a46874-6ba2-46ab-a772-74f750a83f77"), "Swojczyce" },
                    { new Guid("12fa45bb-787b-4b32-b435-eb5d5f2e8276"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Strachocin", new Guid("33a46874-6ba2-46ab-a772-74f750a83f77"), "Strachocin" },
                    { new Guid("00f44bb0-0e67-4e1b-9458-ff6aecaa7e37"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Klokoczyce", new Guid("1cd70b69-127f-4deb-a5cb-186c89a73861"), "Kłokoczyce" },
                    { new Guid("911aac44-915c-4312-8480-231de7ca5140"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zgorzelisko", new Guid("1cd70b69-127f-4deb-a5cb-186c89a73861"), "Zgorzelisko" },
                    { new Guid("fcad7d3e-c3cd-4887-8e55-bf81f38b7699"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zakrzow", new Guid("1cd70b69-127f-4deb-a5cb-186c89a73861"), "Zakrzów" },
                    { new Guid("036e6350-60cc-4663-b407-52eb10ad20dc"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Poswietne", new Guid("1c6699c2-d11f-41a1-bc25-69ec07279176"), "Poświętne" },
                    { new Guid("7b46fbe5-b2e9-44b9-8744-761151fdb951"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Polanowice", new Guid("1c6699c2-d11f-41a1-bc25-69ec07279176"), "Polanowice" },
                    { new Guid("d0eeee0a-6b52-4d23-b089-23e87b2a2666"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Ligota", new Guid("1c6699c2-d11f-41a1-bc25-69ec07279176"), "Ligota" },
                    { new Guid("02639180-bbb6-4e94-9ac6-797e4992d537"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Redzin", new Guid("7930362c-3810-43d6-a8a6-61f7bee04846"), "Rędzin" },
                    { new Guid("62938578-400f-41b2-a1aa-f4b106a757b8"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Osobowice", new Guid("7930362c-3810-43d6-a8a6-61f7bee04846"), "Osobowice" },
                    { new Guid("916e4be4-3b01-413d-ac4d-6ee95e5561e1"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Polanka", new Guid("2fe676de-ebef-4e28-a012-61da79da6947"), "Polanka" },
                    { new Guid("288d3d2c-22d2-4553-80f1-fbda871f445c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Mirowiec", new Guid("2fe676de-ebef-4e28-a012-61da79da6947"), "Mirowiec" },
                    { new Guid("96728063-86a3-44a9-b152-1f47a3319d28"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Rozanka", new Guid("2fe676de-ebef-4e28-a012-61da79da6947"), "Różanka" },
                    { new Guid("a042891b-3447-4381-b6d7-ba5918d16781"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Karlowice", new Guid("2fe676de-ebef-4e28-a012-61da79da6947"), "Karłowice" },
                    { new Guid("1c2dfb3d-1792-441c-976e-e958e007d9a3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Dworek", new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e"), "Dworek" },
                    { new Guid("af2687c8-b22b-4a50-9415-0d80a32dab06"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Poludnie", new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e"), "Południe" },
                    { new Guid("1f7169fa-0b63-473b-a47a-107073cefa8f"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Grabiszyn", new Guid("c75ff32f-6ee3-4347-86b4-1aa0a96651a7"), "Grabiszyn" },
                    { new Guid("4b934a34-90af-43b0-b105-be7e72906468"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Popowice", new Guid("e5e77c3d-bb85-4c7a-868e-43e5d6f623cc"), "Popowice" },
                    { new Guid("57de3b20-b1c7-4a77-96f7-9ce9c633ac7e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Gadow-Maly", new Guid("e5e77c3d-bb85-4c7a-868e-43e5d6f623cc"), "Gądów Mały" },
                    { new Guid("8932a433-c5b2-4c21-80ae-2f9e018f4a3e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zacisze-Zalesie-Szczytniki", new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Zacisze-Zalesie-Szczytniki" },
                    { new Guid("88dd1ffd-00f4-469b-ae94-745e2a336ddb"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Huby", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Huby" },
                    { new Guid("69c05563-4af9-4552-b43d-b742dc33a91f"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Gaj", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Gaj" },
                    { new Guid("a25aa6e2-70dd-413f-9b00-949042b14f97"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Brochow", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Brochów" },
                    { new Guid("7f7e9487-cb8d-489b-8a58-827756622895"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Borek", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Borek" },
                    { new Guid("853f916a-3fbd-4baf-8a09-f29eee498966"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Bienkowice", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Bieńkowice" },
                    { new Guid("7dea11fb-65ed-4b98-a06c-cfd01917b655"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zerniki", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Żerniki" },
                    { new Guid("e8858102-054a-4779-9760-7380ecf08df3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Pracze-Odrzanskie", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Pracze Odrzańskie" },
                    { new Guid("1019e1be-57d7-4ca6-9b38-f5da790eaa33"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Pilczyce-Kozanow-Popowice-Polnocne", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Pilczyce-Kozanów-Popowice Północne" },
                    { new Guid("66f68e3a-50cf-49cd-8191-e1fc627ea2e4"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Oporow", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Oporów" },
                    { new Guid("aa5f1748-d858-45c6-b1bd-fd68d98c8b38"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Nowy-Dwor", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Nowy Dwór" },
                    { new Guid("96adb77d-8505-46fc-9515-41fee1f06a6a"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Muchobor-Wielki", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Muchobór Wielki" },
                    { new Guid("a45a6df5-4695-4db2-a362-c0477eab1c8b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Muchobor-Maly", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Muchobór Mały" },
                    { new Guid("4a7cfa64-3c42-46d7-b694-f6b8eccb5ab6"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Maslice", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Maślice" },
                    { new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Lesnica", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Leśnica" },
                    { new Guid("f6c8b915-38cd-44d8-878d-115cc440c5d3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Kuzniki", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Kuźniki" },
                    { new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Jerzmanowo-Jarnoltow-Strachowice-Osiniec", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Jerzmanowo-Jarnołtów-Strachowice-Osiniec" },
                    { new Guid("c75ff32f-6ee3-4347-86b4-1aa0a96651a7"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Grabiszyn-Grabiszynek", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Grabiszyn-Grabiszynek" },
                    { new Guid("e5e77c3d-bb85-4c7a-868e-43e5d6f623cc"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Gadow-Popowice-Poludniowe", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Gądów-Popowice Południowe" },
                    { new Guid("882cc824-1d0b-4acd-89d0-c846b98ab946"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Gajowice", new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Gajowice" },
                    { new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Srodmiescie", null, "Śródmieście" },
                    { new Guid("0a098de4-92e4-4ef8-887a-141aaf11f8dc"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Stare-Miasto", null, "Stare Miasto" },
                    { new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Psie-Pole", null, "Psie Pole" },
                    { new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Krzyki", null, "Krzyki" },
                    { new Guid("0a77f641-5227-4f4e-8f89-91d431218daa"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Jagodno", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Jagodno" },
                    { new Guid("4e9d230e-ce82-46c2-8024-0044e57e1c62"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zacisze", new Guid("8932a433-c5b2-4c21-80ae-2f9e018f4a3e"), "Zacisze" },
                    { new Guid("59a2d9a7-e6dd-4b8e-b1e7-47e88f18e0dc"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Klecina", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Klecina" },
                    { new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Ksieze", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Księże" },
                    { new Guid("918ce4d5-989f-4458-acc2-e00509b23a20"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Plac-Grunwaldzki", new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Plac Grunwaldzki" },
                    { new Guid("1c2ef079-52ef-4568-a06e-f882dcf28c23"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Olbin", new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Ołbin" },
                    { new Guid("6d9ed1ea-dda0-46c7-9649-4f18f682efda"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Nadodrze", new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Nadodrze" },
                    { new Guid("d83841fa-27fb-4f89-aab2-c40088f75980"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Biskupin-Sepolno-Dabie-Bartoszowice", new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Biskupin-Sępolno-Dąbie-Bartoszowice" },
                    { new Guid("ceb9a82f-0141-49ad-9927-8aeae16da244"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Szczepin", new Guid("0a098de4-92e4-4ef8-887a-141aaf11f8dc"), "Szczepin" },
                    { new Guid("1d087abe-fee0-46b9-9212-c72d4bad5c0b"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Przedmiescie-Swidnickie", new Guid("0a098de4-92e4-4ef8-887a-141aaf11f8dc"), "Przedmieście Świdnickie" },
                    { new Guid("9446441a-79d3-4b14-949d-384ad704165d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Widawa", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Widawa" },
                    { new Guid("2a5a62cf-051f-4270-a35b-8ec4a106c522"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Swiniary", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Świniary" },
                    { new Guid("33a46874-6ba2-46ab-a772-74f750a83f77"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Swojczyce-Strachocin-Wojnow", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Swojczyce-Strachocin-Wojnów" },
                    { new Guid("90c8dcff-3eda-4776-ba1e-fc505bcb89fd"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Soltysowice", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Sołtysowice" },
                    { new Guid("1cd70b69-127f-4deb-a5cb-186c89a73861"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Psie-Pole-Zawidawie", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Psie Pole Zawidawie" },
                    { new Guid("1c6699c2-d11f-41a1-bc25-69ec07279176"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Polanowice-Poswietne-Ligota", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Polanowice-Poświętne-Ligota" },
                    { new Guid("abe94cf7-c965-4223-b7df-be2b72f0fe28"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Pawlowice", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Pawłowice" },
                    { new Guid("7930362c-3810-43d6-a8a6-61f7bee04846"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Osobowice-Redzin", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Osobowice-Rędzin" },
                    { new Guid("3b4ea3b0-cd9a-43a9-8af5-9636afaf82a1"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Lipa-Piotrowska", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Lipa Piotrowska" },
                    { new Guid("ffe59d30-48ca-4534-a798-5406de30775d"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Kowale", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Kowale" },
                    { new Guid("29b9a280-a2a5-4436-810c-78df10740360"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Kleczkow", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Kleczków" },
                    { new Guid("2fe676de-ebef-4e28-a012-61da79da6947"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Karlowice-Rozanka", new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Karłowice-Różanka" },
                    { new Guid("a63d5148-471b-422c-b016-0317c9689cd3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Wojszyce", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Wojszyce" },
                    { new Guid("86c0e69c-1259-4245-b94f-9c33d12aa490"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Tarnogaj", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Tarnogaj" },
                    { new Guid("ddcc1be6-395e-43aa-ad72-a150b868f7f3"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Przedmiescie-Olawskie", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Przedmieście Oławskie" },
                    { new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Powstancow-Slaskich", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Powstańców Śląskich" },
                    { new Guid("25fb993b-700f-45be-8699-c09881f7a76c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Oltaszyn", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Ołtaszyn" },
                    { new Guid("ac5d8893-9370-4546-bbb5-51edc01a442c"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Krzyki-Partynice", new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Krzyki-Partynice" },
                    { new Guid("624bd6bd-e92c-4358-a021-269d791b7884"), new Guid("1d02ce58-3bfe-46bc-aa61-d2de1c1690c1"), "Zalesie", new Guid("8932a433-c5b2-4c21-80ae-2f9e018f4a3e"), "Zalesie" }
                });

            migrationBuilder.InsertData(
                table: "CityDistrictNameVariants",
                columns: new[] { "Id", "CityDistrictId", "Value" },
                values: new object[,]
                {
                    { new Guid("66d75b75-f425-49d2-87bd-bd109aca8a6d"), new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889"), "Fabrycznej" },
                    { new Guid("19b50d37-d315-45cc-873f-d6b0e7d51c59"), new Guid("9cee8e4c-89bf-4cbc-8476-b130dabae470"), "Bierdzanach" },
                    { new Guid("5cc8a418-24b3-48ea-9438-b7c51818b84f"), new Guid("ba687519-0138-40e2-ac65-77d297c4955d"), "Opatowicach" },
                    { new Guid("12346d4c-6726-4531-a24f-ddfbef4abcc3"), new Guid("cf8556b4-eaef-4eed-ba5e-98ce8eb3822b"), "Świątnikach" },
                    { new Guid("ee076fa3-7401-418e-9fe0-a22d16953535"), new Guid("cf8556b4-eaef-4eed-ba5e-98ce8eb3822b"), "Swiatnikach" },
                    { new Guid("2aa68cd4-36aa-430f-b48d-cb147627c401"), new Guid("b2669d90-3214-49f7-b17b-f974e3b8f22c"), "Księżach" },
                    { new Guid("7f35c1d1-e12a-4f78-b976-adfc3efb10c2"), new Guid("b2669d90-3214-49f7-b17b-f974e3b8f22c"), "Ksiezach" },
                    { new Guid("0f938c04-e8bf-4af2-9895-608c90a11286"), new Guid("b2669d90-3214-49f7-b17b-f974e3b8f22c"), "Księżach wielkich" },
                    { new Guid("b3d999b4-1031-48bf-9b77-847615bad75a"), new Guid("b2669d90-3214-49f7-b17b-f974e3b8f22c"), "Ksiezach wielkich" },
                    { new Guid("0fba6ccd-b37a-4359-a94c-c169b8b2af8d"), new Guid("2615becf-bbc6-4ad6-b37b-68f1d3b9265d"), "Księżach" },
                    { new Guid("4fbfa51b-c049-45fd-88e8-45fd03cdd819"), new Guid("2615becf-bbc6-4ad6-b37b-68f1d3b9265d"), "Ksiezach" },
                    { new Guid("555cb670-935c-4bbd-b773-5f25271103ec"), new Guid("2615becf-bbc6-4ad6-b37b-68f1d3b9265d"), "Księżach małych" },
                    { new Guid("0a0421bd-f92e-40ec-aa81-654d9c796e38"), new Guid("2615becf-bbc6-4ad6-b37b-68f1d3b9265d"), "Ksiezach malych" },
                    { new Guid("22b24ff3-3f3c-438e-a1a9-cf3eded4650a"), new Guid("eff858bd-c899-4a2c-80ad-f6800ea0de14"), "Partynicach" },
                    { new Guid("a2100738-eb14-47e0-9196-cb69461412e7"), new Guid("4d965d02-a05a-4c4b-a8c3-e9504b518185"), "Gliniankach" },
                    { new Guid("6d5ce240-f42a-464c-937e-b5f20551ceea"), new Guid("2b25ded6-a25f-441d-b8b8-53d7b7a8e255"), "Nowej karczmie" },
                    { new Guid("87e02521-d428-4e7c-804f-cd0dfa08aa34"), new Guid("3a91b41d-5cf7-4480-bb5c-ed9179d7c00b"), "Janówku" },
                    { new Guid("45dfb37b-fbb4-4e5d-87b6-fed3935319d2"), new Guid("3a91b41d-5cf7-4480-bb5c-ed9179d7c00b"), "Janowku" },
                    { new Guid("a424b69a-e8c3-47ac-ac7e-4e72a47d8471"), new Guid("c91dc887-4deb-4274-abe3-be0baed37a33"), "Popowicach" },
                    { new Guid("0a8d4b73-65dc-4b62-b350-d738bf4e7f19"), new Guid("4f2a852b-2c4c-44ca-b5ad-646221313c4c"), "Pilczycach" },
                    { new Guid("613ea767-69f2-47db-bbe9-4e6b5387587d"), new Guid("a4ce962e-5246-4952-80ae-0087f1266700"), "Gajowej" },
                    { new Guid("0b8cff6a-ebc7-4ced-a7b5-fa4ae46df7d3"), new Guid("7dec37f3-d52d-477d-a292-ee1f617a6b3e"), "Pustkach" },
                    { new Guid("8862266f-3ab4-4d52-b37a-27a59f9d99f7"), new Guid("ab96fd01-5a2d-4f2a-9b89-626b092ae4cb"), "Mokrej" },
                    { new Guid("cec42b5e-3948-454d-92e4-449fcc02cf1e"), new Guid("4e7c7585-be5a-4d4d-a87b-0ec49f32fd4c"), "Ratuniu" },
                    { new Guid("957868bc-3233-453b-a543-768889c19e95"), new Guid("caceb67b-1738-459d-a8f7-a5482604bfdd"), "Marszowicach" },
                    { new Guid("beafde37-3210-4660-9741-521e82b1783b"), new Guid("6f8f3d9e-dcec-42f0-bec1-09a825cc3930"), "Złotnikach" },
                    { new Guid("24a63bab-fad8-4bf8-a597-cf42be71ac22"), new Guid("6f8f3d9e-dcec-42f0-bec1-09a825cc3930"), "Zlotnikach" },
                    { new Guid("e8da6925-f255-478f-b819-9868a547c995"), new Guid("852cb711-daae-4cca-b65b-62283725e5da"), "Stabłowicach" },
                    { new Guid("0651439c-bf20-45d2-9ceb-ee5860a036cd"), new Guid("d8abcb58-1ee0-45dc-bc44-633660d09d75"), "Nowym domie" },
                    { new Guid("e49553df-fd18-4191-8a81-4507163bb5d8"), new Guid("852cb711-daae-4cca-b65b-62283725e5da"), "Stablowicach" },
                    { new Guid("fa14c562-e3c8-4a0f-ac77-2b54c7a9567f"), new Guid("af2687c8-b22b-4a50-9415-0d80a32dab06"), "Poludniu" },
                    { new Guid("d210a28a-04ac-4f90-9697-955048e215a9"), new Guid("1c2dfb3d-1792-441c-976e-e958e007d9a3"), "Dworku" },
                    { new Guid("efae3434-18c0-4522-8e73-78af7e558156"), new Guid("c566fd0d-0873-4f6f-8b0a-070ea521dc93"), "Szczytnikach" },
                    { new Guid("9d4f921e-d7e2-4850-9130-9755688c17cf"), new Guid("c4de6e53-8b6e-45e1-a587-7fc5ae2ef8ac"), "Sępolnie" },
                    { new Guid("af49739d-41ae-4c00-a285-5ebea27c1580"), new Guid("c4de6e53-8b6e-45e1-a587-7fc5ae2ef8ac"), "Sepolnie" },
                    { new Guid("1245afd8-097f-4270-b123-cd6c49c4b005"), new Guid("69838cb4-81f9-4e05-a50d-d224a039ce89"), "Dąbiu" },
                    { new Guid("76c5dafb-9e64-4bd1-84f3-afd77f63a394"), new Guid("69838cb4-81f9-4e05-a50d-d224a039ce89"), "Dabiu" },
                    { new Guid("cc7060d7-6402-4211-887c-d774c2880534"), new Guid("69838cb4-81f9-4e05-a50d-d224a039ce89"), "Dąbiach" },
                    { new Guid("2492d889-2847-4e9e-8983-d88e2ae9a388"), new Guid("69838cb4-81f9-4e05-a50d-d224a039ce89"), "Dabiach" },
                    { new Guid("e31e1cd0-f62b-495c-a51f-6d1afaeff0d5"), new Guid("830582c7-1783-4610-a6c1-804e6af3c2f8"), "Bartoszowicach" },
                    { new Guid("1152ab6d-77ad-4923-9712-d56d4c0c307d"), new Guid("74e43ed2-2357-4895-afbd-836cd2b9cc5e"), "Popielach" },
                    { new Guid("55e05adc-8f32-405d-810e-196eb539cd6a"), new Guid("7ca1f8fc-fc80-4758-a884-c231d2d74934"), "Wojnowie" },
                    { new Guid("c9105703-487c-4ac0-879f-f6c7f83c1274"), new Guid("6b2513f1-c8c2-4bf3-b5de-7dbb43a8ab81"), "Swojszycach" },
                    { new Guid("66beb497-5b50-4555-b506-6da578d529cc"), new Guid("00f44bb0-0e67-4e1b-9458-ff6aecaa7e37"), "Kłokoczycach" },
                    { new Guid("1b49dee5-b31f-42a4-b684-4ff375852958"), new Guid("00f44bb0-0e67-4e1b-9458-ff6aecaa7e37"), "Klokoczycach" },
                    { new Guid("18a0f2a3-4a9f-4d9c-8e54-a468f73e3016"), new Guid("911aac44-915c-4312-8480-231de7ca5140"), "Zgorzelisku" },
                    { new Guid("720e49e0-7d45-4b31-9cbd-ee99ea23944e"), new Guid("fcad7d3e-c3cd-4887-8e55-bf81f38b7699"), "Zakrzowie" },
                    { new Guid("b8223315-487f-42dd-a0e4-079f8a9f9ed5"), new Guid("036e6350-60cc-4663-b407-52eb10ad20dc"), "Poświętnej" },
                    { new Guid("5bf332e3-2d61-485b-8bb4-d7f8e3f94fc3"), new Guid("036e6350-60cc-4663-b407-52eb10ad20dc"), "Poswietnej" },
                    { new Guid("841c651c-fb9a-401e-815f-cd6ab7bca735"), new Guid("036e6350-60cc-4663-b407-52eb10ad20dc"), "Poświętnym" },
                    { new Guid("40ac09e2-cf6b-409f-b615-0e9436ae298d"), new Guid("036e6350-60cc-4663-b407-52eb10ad20dc"), "Poswietnym" },
                    { new Guid("12a9bc9a-4207-4d0e-b1db-967050d4cdd7"), new Guid("7b46fbe5-b2e9-44b9-8744-761151fdb951"), "Polanowicach" },
                    { new Guid("c9d79cef-309e-422a-825f-cf1a02f8e31d"), new Guid("d0eeee0a-6b52-4d23-b089-23e87b2a2666"), "Ligocie" },
                    { new Guid("a828a718-9827-47f2-bd1e-9bd7df8b03a5"), new Guid("916e4be4-3b01-413d-ac4d-6ee95e5561e1"), "Polance" },
                    { new Guid("4183882f-72ca-4d0c-a28e-eaede21d9ea3"), new Guid("288d3d2c-22d2-4553-80f1-fbda871f445c"), "Mirowcu" },
                    { new Guid("8de4117c-6c9e-4ff8-a207-ae26e8a98b47"), new Guid("96728063-86a3-44a9-b152-1f47a3319d28"), "Różance" },
                    { new Guid("cef3a3e1-7b49-42a6-89ad-a63b35e4cb24"), new Guid("96728063-86a3-44a9-b152-1f47a3319d28"), "Rozance" },
                    { new Guid("c7024bc0-706e-4207-8ed3-458b6ae0a5b0"), new Guid("a042891b-3447-4381-b6d7-ba5918d16781"), "Karłowicach" },
                    { new Guid("b75d510f-974f-46c9-a8e5-263deb2ec7f9"), new Guid("a042891b-3447-4381-b6d7-ba5918d16781"), "Karlowicach" },
                    { new Guid("27ca7cbe-07c4-46f8-b6a3-9c6583121c0e"), new Guid("af2687c8-b22b-4a50-9415-0d80a32dab06"), "Południu" },
                    { new Guid("8d09ca52-81f3-4081-a9a6-b0a38f061054"), new Guid("4e9d230e-ce82-46c2-8024-0044e57e1c62"), "Zaciszu" },
                    { new Guid("a5786af0-e2fd-4799-a008-0f15f512b66d"), new Guid("e660b554-5262-41bc-bf85-394fd6e281d0"), "Osińcu" },
                    { new Guid("c59474af-af99-4ce2-8ee1-8b0dbf3eb1f0"), new Guid("30fcd3fe-1733-49f6-b0ff-b58586454c86"), "Strachowicach" },
                    { new Guid("74cbad72-cc4d-49ca-bb38-2dc5e3cb63ac"), new Guid("88dd1ffd-00f4-469b-ae94-745e2a336ddb"), "Hubach" },
                    { new Guid("0aaa7369-4cbc-4dfc-b5fb-49a5da711793"), new Guid("7f7e9487-cb8d-489b-8a58-827756622895"), "Borku" },
                    { new Guid("d91def1f-d742-46b6-bf5d-d4a69eb5854d"), new Guid("853f916a-3fbd-4baf-8a09-f29eee498966"), "Bieńkowicach" },
                    { new Guid("de46a69d-06c5-450c-8897-f7f4caf216ca"), new Guid("853f916a-3fbd-4baf-8a09-f29eee498966"), "Bienkowicach" },
                    { new Guid("5566cab4-b65c-4e46-9f79-ec340c3d4912"), new Guid("7dea11fb-65ed-4b98-a06c-cfd01917b655"), "Żernikach" },
                    { new Guid("14bf2192-8222-41bd-830f-edcd5dc1cd2f"), new Guid("7dea11fb-65ed-4b98-a06c-cfd01917b655"), "Zernikach" },
                    { new Guid("79f208ef-ad26-4d97-b5ed-b8fb27af6d70"), new Guid("e8858102-054a-4779-9760-7380ecf08df3"), "Praczu Odrzańskim" },
                    { new Guid("36f80b74-e20d-4259-858a-f2b93968e225"), new Guid("e8858102-054a-4779-9760-7380ecf08df3"), "Praczu Odrzanskim" },
                    { new Guid("cae1802d-4284-436b-a596-4b7d8a21545d"), new Guid("aa5f1748-d858-45c6-b1bd-fd68d98c8b38"), "Nowy dwor" },
                    { new Guid("36876439-f286-42d9-8eba-857629dcee60"), new Guid("aa5f1748-d858-45c6-b1bd-fd68d98c8b38"), "Nowym dworze" },
                    { new Guid("a3dbdf7a-dec2-4a7b-892b-0853a5fde42b"), new Guid("96adb77d-8505-46fc-9515-41fee1f06a6a"), "Muchobór" },
                    { new Guid("e10eec4e-d540-4fd5-b278-e57d3d39feeb"), new Guid("96adb77d-8505-46fc-9515-41fee1f06a6a"), "Muchobor" },
                    { new Guid("69b7af3b-0993-4d4b-9cfc-5081c80fd9bc"), new Guid("a45a6df5-4695-4db2-a362-c0477eab1c8b"), "Muchobór" },
                    { new Guid("06d32d31-64fa-4df1-9817-67fbc7686775"), new Guid("a45a6df5-4695-4db2-a362-c0477eab1c8b"), "Muchobor" },
                    { new Guid("031211be-eb65-4e62-8c8c-e457dcaf014e"), new Guid("4a7cfa64-3c42-46d7-b694-f6b8eccb5ab6"), "Maślicach" },
                    { new Guid("0b7bee7e-61d6-457e-9553-3640b3bd86c3"), new Guid("4a7cfa64-3c42-46d7-b694-f6b8eccb5ab6"), "Maslicach" },
                    { new Guid("00647eee-a5a5-4756-81d8-fa1747055e98"), new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Leśnicy" },
                    { new Guid("8143a298-10ad-4930-8cb3-ca56b1f648c0"), new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742"), "Lesnicy" },
                    { new Guid("6664771d-f04c-49a6-8f17-99942bd7ca88"), new Guid("f6c8b915-38cd-44d8-878d-115cc440c5d3"), "Kuźnikach" },
                    { new Guid("9887aa49-d6ba-4ae8-9a02-a68ed4adfaae"), new Guid("f6c8b915-38cd-44d8-878d-115cc440c5d3"), "Kuznikach" },
                    { new Guid("c1508b6a-185b-4220-84b6-e2a2e6a20a44"), new Guid("882cc824-1d0b-4acd-89d0-c846b98ab946"), "Gajowicach" },
                    { new Guid("13f00177-c102-452e-a400-dff52e62960e"), new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Śródmieściu" },
                    { new Guid("0506708d-1928-47d9-84ac-cae51292069c"), new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d"), "Srodmiesciu" },
                    { new Guid("e39a663c-5144-4ed7-a56a-dfe6569a3362"), new Guid("0a098de4-92e4-4ef8-887a-141aaf11f8dc"), "Starym mieście" },
                    { new Guid("2818fd91-86cc-44ab-91e9-72e90449457f"), new Guid("0a098de4-92e4-4ef8-887a-141aaf11f8dc"), "Starym miescie" },
                    { new Guid("d1c5265d-4c56-448a-bdbd-434ab0b105c0"), new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab"), "Psim polu" },
                    { new Guid("48ba02dc-da6d-4db3-a2b8-6838d658d1e2"), new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94"), "Krzykach" },
                    { new Guid("e1e41732-480f-4b65-b3c1-18a71de09ec1"), new Guid("0a77f641-5227-4f4e-8f89-91d431218daa"), "Jagodnie" },
                    { new Guid("95b5989f-8a0f-4b19-912b-b024b9c919d3"), new Guid("e660b554-5262-41bc-bf85-394fd6e281d0"), "Osincu" },
                    { new Guid("5b0c2ff8-1492-4fb6-97ea-ba9d0b502ff5"), new Guid("59a2d9a7-e6dd-4b8e-b1e7-47e88f18e0dc"), "Klecinie" },
                    { new Guid("f97797c4-93a3-4793-9fb6-d909de3d7d30"), new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Księżach" },
                    { new Guid("a9d5e393-27d8-4d24-b008-30470d46535e"), new Guid("a1c44a64-da37-4dae-b099-0f952f259a56"), "Jarnołtówku" },
                    { new Guid("4d4a90d6-773b-4e84-a0f6-07f55cff2f61"), new Guid("a1c44a64-da37-4dae-b099-0f952f259a56"), "Jarnoltowku" },
                    { new Guid("8dc8b068-bb87-4b46-809b-a45781f58f96"), new Guid("9b465ded-eed0-4ea4-9bf4-ebf3a642dd4b"), "Jerzmanowej" },
                    { new Guid("3d306915-b766-420a-8993-82bbb779718d"), new Guid("914214ae-cd23-4a67-8af6-0cf580813b6c"), "Grabiszynku" },
                    { new Guid("c97df31e-3a4e-4489-94b5-18cf7fe09b34"), new Guid("1f7169fa-0b63-473b-a47a-107073cefa8f"), "Grabiszynie" },
                    { new Guid("102375d5-bad8-424e-ab9d-78bc8a03de5e"), new Guid("4b934a34-90af-43b0-b105-be7e72906468"), "Popowicach" },
                    { new Guid("555faae5-c3ea-46c2-a312-3828b0cd05cf"), new Guid("57de3b20-b1c7-4a77-96f7-9ce9c633ac7e"), "Gądowie" },
                    { new Guid("fa2ab82d-7261-4d49-b05e-8164018bdb09"), new Guid("57de3b20-b1c7-4a77-96f7-9ce9c633ac7e"), "Gadowie" },
                    { new Guid("fd6f22bc-a510-4795-b02f-2b33ffa84bd1"), new Guid("57de3b20-b1c7-4a77-96f7-9ce9c633ac7e"), "Gadowie małym" },
                    { new Guid("ce9dc855-4083-467e-98fa-45d5b8067bee"), new Guid("57de3b20-b1c7-4a77-96f7-9ce9c633ac7e"), "Gadowie malym" },
                    { new Guid("f70d50cd-f0f9-40c1-8417-c895ae78d0dd"), new Guid("918ce4d5-989f-4458-acc2-e00509b23a20"), "Placu grunwaldzkim" },
                    { new Guid("f08a8949-ac74-41ca-bf74-ce62f3b5c684"), new Guid("6d9ed1ea-dda0-46c7-9649-4f18f682efda"), "Nadodrzu" },
                    { new Guid("038c7e67-924c-4eae-9ed7-ad6e4aa5f77d"), new Guid("1d087abe-fee0-46b9-9212-c72d4bad5c0b"), "Przedmieściu świdnickim" },
                    { new Guid("9d672522-5fdf-42c1-b1e2-564d76975d3a"), new Guid("1d087abe-fee0-46b9-9212-c72d4bad5c0b"), "Przedmiesciu swidnickim" },
                    { new Guid("33e50a7d-a7b4-4941-8c25-c922639b007d"), new Guid("9446441a-79d3-4b14-949d-384ad704165d"), "Widawie" },
                    { new Guid("1362264a-2684-493c-8115-79a400c3454f"), new Guid("2a5a62cf-051f-4270-a35b-8ec4a106c522"), "Świniarach" },
                    { new Guid("bf4a74ab-550c-4ecb-9ba7-40faf3fce1e2"), new Guid("2a5a62cf-051f-4270-a35b-8ec4a106c522"), "Swiniarach" },
                    { new Guid("c50193a4-a54b-4d14-8187-6b748b436818"), new Guid("90c8dcff-3eda-4776-ba1e-fc505bcb89fd"), "Sołtysowicach" },
                    { new Guid("1a40665c-9f74-4082-ab12-37cf1159a17c"), new Guid("90c8dcff-3eda-4776-ba1e-fc505bcb89fd"), "Soltysowicach" },
                    { new Guid("07e84e5a-3a55-4c12-a880-5bd56427c725"), new Guid("abe94cf7-c965-4223-b7df-be2b72f0fe28"), "Pawłowicach" },
                    { new Guid("2fe7c271-1988-436e-bd5e-c3caaaa28155"), new Guid("abe94cf7-c965-4223-b7df-be2b72f0fe28"), "Pawlowicach" },
                    { new Guid("8553f3fb-8859-4299-b0cc-a4dc8b38af51"), new Guid("3b4ea3b0-cd9a-43a9-8af5-9636afaf82a1"), "Lipie piotrowskiej" },
                    { new Guid("adc28067-787f-4b88-9aa4-f613a503237d"), new Guid("ffe59d30-48ca-4534-a798-5406de30775d"), "Kowalach" },
                    { new Guid("8d1c7a31-1459-42ff-a056-8ef428f9eacf"), new Guid("a63d5148-471b-422c-b016-0317c9689cd3"), "Wojszycach" },
                    { new Guid("75b9425a-4be8-4353-b9c3-9562454e56a2"), new Guid("ddcc1be6-395e-43aa-ad72-a150b868f7f3"), "Przedmieściu oławskim" },
                    { new Guid("398c9bd5-dd85-438c-9715-107372d2f301"), new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e"), "Przedmiesciu olawskim" },
                    { new Guid("fcc80916-8945-4b95-9217-566da6904d49"), new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e"), "Powstancow slaskich" },
                    { new Guid("f2b3de61-5df5-477f-ad14-b3e0e11a1adc"), new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915"), "Ksiezach" },
                    { new Guid("68524e8d-dac4-449d-bab5-587ce356b222"), new Guid("624bd6bd-e92c-4358-a021-269d791b7884"), "Zalesiu" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityDistrictNameVariants_CityDistrictId",
                table: "CityDistrictNameVariants",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_CityDistrictNameVariants_Value_CityDistrictId",
                table: "CityDistrictNameVariants",
                columns: new[] { "Value", "CityDistrictId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_CityId",
                table: "CityDistricts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_Name_ParentId_CityId",
                table: "CityDistricts",
                columns: new[] { "Name", "ParentId", "CityId" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CityDistricts_PolishName_ParentId_CityId",
                table: "CityDistricts",
                columns: new[] { "PolishName", "ParentId", "CityId" },
                unique: true,
                filter: "[ParentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityDistrictNameVariants");

            migrationBuilder.DropTable(
                name: "CityDistricts");
        }
    }
}

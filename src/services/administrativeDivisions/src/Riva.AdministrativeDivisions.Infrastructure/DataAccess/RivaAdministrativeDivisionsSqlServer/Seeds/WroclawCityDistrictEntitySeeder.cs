using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds
{
    public static class WroclawCityDistrictEntitySeeder
    {
        private static readonly Guid GadowPopowicePoludnioweId = new Guid("e5e77c3d-bb85-4c7a-868e-43e5d6f623cc");
        private static readonly Guid GrabiszynGrabiszynekId = new Guid("c75ff32f-6ee3-4347-86b4-1aa0a96651a7");
        private static readonly Guid JerzmanowoJarnoltowStrachowiceOsiniecId = new Guid("e3496408-fce3-4fee-b42b-3b0f529426e9");
        private static readonly Guid PilczyceKozanowPopowicePolnocneId = new Guid("1019e1be-57d7-4ca6-9b38-f5da790eaa33");
        private static readonly Guid KrzykiPartyniceId = new Guid("ac5d8893-9370-4546-bbb5-51edc01a442c");
        private static readonly Guid KarlowiceRozankaId = new Guid("2fe676de-ebef-4e28-a012-61da79da6947");
        private static readonly Guid OsobowiceRedzinId = new Guid("7930362c-3810-43d6-a8a6-61f7bee04846");
        private static readonly Guid PolanowicePoswietneLigotaId = new Guid("1c6699c2-d11f-41a1-bc25-69ec07279176");
        private static readonly Guid PsiePoleZawidawieId = new Guid("1cd70b69-127f-4deb-a5cb-186c89a73861");
        private static readonly Guid SwojczyceStrachocinWojnowId = new Guid("33a46874-6ba2-46ab-a772-74f750a83f77");
        private static readonly Guid BiskupinSepolnoDabieBartoszowiceId = new Guid("d83841fa-27fb-4f89-aab2-c40088f75980");
        private static readonly Guid ZaciszeZalesieSzczytnikiId = new Guid("8932a433-c5b2-4c21-80ae-2f9e018f4a3e");

        public static readonly Guid FabrycznaId = new Guid("6dacf67a-ba5f-4bff-ac7f-95ed6fd4f889");
        public static readonly Guid KrzykiId = new Guid("21db1b78-4a25-4f04-a6e3-31314bc45f94");
        public static readonly Guid PsiePoleId = new Guid("fc53428e-b09a-44fa-bff7-44379a6240ab");
        public static readonly Guid StareMiastoId = new Guid("0a098de4-92e4-4ef8-887a-141aaf11f8dc");
        public static readonly Guid SrodmiescieId = new Guid("0a6a49ca-7508-49a8-bfae-3a995076943d");

        public static readonly Guid GajowiceId = new Guid("882cc824-1d0b-4acd-89d0-c846b98ab946");
        public static readonly Guid KuznikiId = new Guid("f6c8b915-38cd-44d8-878d-115cc440c5d3");
        public static readonly Guid LesnicaId = new Guid("c24e2403-5fc2-4e1a-bbf5-d4c510847742");
        public static readonly Guid MasliceId = new Guid("4a7cfa64-3c42-46d7-b694-f6b8eccb5ab6");
        public static readonly Guid MuchoborMalyId = new Guid("a45a6df5-4695-4db2-a362-c0477eab1c8b");
        public static readonly Guid MuchoborWielkiId = new Guid("96adb77d-8505-46fc-9515-41fee1f06a6a");
        public static readonly Guid NowyDworId = new Guid("aa5f1748-d858-45c6-b1bd-fd68d98c8b38");
        public static readonly Guid PraczeOdrzanskieId = new Guid("e8858102-054a-4779-9760-7380ecf08df3");
        public static readonly Guid ZernikiId = new Guid("7dea11fb-65ed-4b98-a06c-cfd01917b655");
        public static readonly Guid BienkowiceId = new Guid("853f916a-3fbd-4baf-8a09-f29eee498966");
        public static readonly Guid BorekId = new Guid("7f7e9487-cb8d-489b-8a58-827756622895");
        public static readonly Guid HubyId = new Guid("88dd1ffd-00f4-469b-ae94-745e2a336ddb");
        public static readonly Guid JagodnoId = new Guid("0a77f641-5227-4f4e-8f89-91d431218daa");
        public static readonly Guid KlecinaId = new Guid("59a2d9a7-e6dd-4b8e-b1e7-47e88f18e0dc");
        public static readonly Guid KsiezeId = new Guid("83bd3deb-0f48-4846-8eaf-54ade5b90915");
        public static readonly Guid PowstancowSlaskichId = new Guid("968cb0ac-d6d7-41ac-9f92-2a88a4c5322e");
        public static readonly Guid PrzedmiescieOlawskieId = new Guid("ddcc1be6-395e-43aa-ad72-a150b868f7f3");
        public static readonly Guid WojszyceId = new Guid("a63d5148-471b-422c-b016-0317c9689cd3");
        public static readonly Guid KowaleId = new Guid("ffe59d30-48ca-4534-a798-5406de30775d");
        public static readonly Guid LipaPiotrowskaId = new Guid("3b4ea3b0-cd9a-43a9-8af5-9636afaf82a1");
        public static readonly Guid PawlowiceId = new Guid("abe94cf7-c965-4223-b7df-be2b72f0fe28");
        public static readonly Guid SoltysowiceId = new Guid("90c8dcff-3eda-4776-ba1e-fc505bcb89fd");
        public static readonly Guid SwiniaryId = new Guid("2a5a62cf-051f-4270-a35b-8ec4a106c522");
        public static readonly Guid WidawaId = new Guid("9446441a-79d3-4b14-949d-384ad704165d");
        public static readonly Guid PrzedmiescieSwidnickieId = new Guid("1d087abe-fee0-46b9-9212-c72d4bad5c0b");
        public static readonly Guid NadodrzeId = new Guid("6d9ed1ea-dda0-46c7-9649-4f18f682efda");
        public static readonly Guid PlacGrunwaldzkiId = new Guid("918ce4d5-989f-4458-acc2-e00509b23a20");

        public static readonly Guid GadowMalyId = new Guid("57de3b20-b1c7-4a77-96f7-9ce9c633ac7e");
        public static readonly Guid PopowicePoludnioweId = new Guid("4b934a34-90af-43b0-b105-be7e72906468");
        public static readonly Guid GrabiszynId = new Guid("1f7169fa-0b63-473b-a47a-107073cefa8f");
        public static readonly Guid GrabiszynekId = new Guid("914214ae-cd23-4a67-8af6-0cf580813b6c");
        public static readonly Guid JerzmanowoId = new Guid("9b465ded-eed0-4ea4-9bf4-ebf3a642dd4b");
        public static readonly Guid JarnoltowId = new Guid("a1c44a64-da37-4dae-b099-0f952f259a56");
        public static readonly Guid StrachowiceId = new Guid("30fcd3fe-1733-49f6-b0ff-b58586454c86");
        public static readonly Guid OsiniecId = new Guid("e660b554-5262-41bc-bf85-394fd6e281d0");
        public static readonly Guid StablowiceId = new Guid("852cb711-daae-4cca-b65b-62283725e5da");
        public static readonly Guid ZlotnikiId = new Guid("6f8f3d9e-dcec-42f0-bec1-09a825cc3930");
        public static readonly Guid MarszowiceId = new Guid("caceb67b-1738-459d-a8f7-a5482604bfdd");
        public static readonly Guid RatynId = new Guid("4e7c7585-be5a-4d4d-a87b-0ec49f32fd4c");
        public static readonly Guid MokraId = new Guid("ab96fd01-5a2d-4f2a-9b89-626b092ae4cb");
        public static readonly Guid PustkiId = new Guid("7dec37f3-d52d-477d-a292-ee1f617a6b3e");
        public static readonly Guid GajowaId = new Guid("a4ce962e-5246-4952-80ae-0087f1266700");
        public static readonly Guid PilczyceId = new Guid("4f2a852b-2c4c-44ca-b5ad-646221313c4c");
        public static readonly Guid PopowicePolnoceId = new Guid("c91dc887-4deb-4274-abe3-be0baed37a33");
        public static readonly Guid JanowekId = new Guid("3a91b41d-5cf7-4480-bb5c-ed9179d7c00b");
        public static readonly Guid NowaKarczmaId = new Guid("2b25ded6-a25f-441d-b8b8-53d7b7a8e255");
        public static readonly Guid GliniankiId = new Guid("4d965d02-a05a-4c4b-a8c3-e9504b518185");
        public static readonly Guid PartyniceId = new Guid("eff858bd-c899-4a2c-80ad-f6800ea0de14");
        public static readonly Guid KsiezeMaleId = new Guid("2615becf-bbc6-4ad6-b37b-68f1d3b9265d");
        public static readonly Guid KsiezeWielkieId = new Guid("b2669d90-3214-49f7-b17b-f974e3b8f22c");
        public static readonly Guid SwiatnikiId = new Guid("cf8556b4-eaef-4eed-ba5e-98ce8eb3822b");
        public static readonly Guid OpatowiceId = new Guid("ba687519-0138-40e2-ac65-77d297c4955d");
        public static readonly Guid BierdzanyId = new Guid("9cee8e4c-89bf-4cbc-8476-b130dabae470");
        public static readonly Guid NowyDomId = new Guid("d8abcb58-1ee0-45dc-bc44-633660d09d75");
        public static readonly Guid PoludnieId = new Guid("af2687c8-b22b-4a50-9415-0d80a32dab06");
        public static readonly Guid DworekId = new Guid("1c2dfb3d-1792-441c-976e-e958e007d9a3");
        public static readonly Guid KarlowiceId = new Guid("a042891b-3447-4381-b6d7-ba5918d16781");
        public static readonly Guid RozankaId = new Guid("96728063-86a3-44a9-b152-1f47a3319d28");
        public static readonly Guid MirowiecId = new Guid("288d3d2c-22d2-4553-80f1-fbda871f445c");
        public static readonly Guid PolankaId = new Guid("916e4be4-3b01-413d-ac4d-6ee95e5561e1");
        public static readonly Guid OsobowiceId = new Guid("62938578-400f-41b2-a1aa-f4b106a757b8");
        public static readonly Guid LigotaId = new Guid("d0eeee0a-6b52-4d23-b089-23e87b2a2666");
        public static readonly Guid PolanowiceId = new Guid("7b46fbe5-b2e9-44b9-8744-761151fdb951");
        public static readonly Guid PoswietneId = new Guid("036e6350-60cc-4663-b407-52eb10ad20dc");
        public static readonly Guid ZakrzowId = new Guid("fcad7d3e-c3cd-4887-8e55-bf81f38b7699");
        public static readonly Guid ZgorzeliskoId = new Guid("911aac44-915c-4312-8480-231de7ca5140");
        public static readonly Guid KlokoczyceId = new Guid("00f44bb0-0e67-4e1b-9458-ff6aecaa7e37");
        public static readonly Guid StrachocinId = new Guid("12fa45bb-787b-4b32-b435-eb5d5f2e8276");
        public static readonly Guid SwojczyceId = new Guid("6b2513f1-c8c2-4bf3-b5de-7dbb43a8ab81");
        public static readonly Guid WojnowId = new Guid("7ca1f8fc-fc80-4758-a884-c231d2d74934");
        public static readonly Guid PopieleId = new Guid("74e43ed2-2357-4895-afbd-836cd2b9cc5e");
        public static readonly Guid BartoszowiceId = new Guid("830582c7-1783-4610-a6c1-804e6af3c2f8");
        public static readonly Guid DabieId = new Guid("69838cb4-81f9-4e05-a50d-d224a039ce89");
        public static readonly Guid SepolnoId = new Guid("c4de6e53-8b6e-45e1-a587-7fc5ae2ef8ac");
        public static readonly Guid SzczytnikiId = new Guid("c566fd0d-0873-4f6f-8b0a-070ea521dc93");
        public static readonly Guid ZaciszeId = new Guid("4e9d230e-ce82-46c2-8024-0044e57e1c62");
        public static readonly Guid ZalesieId = new Guid("624bd6bd-e92c-4358-a021-269d791b7884");


        private static readonly IEnumerable<CityDistrictEntity> WroclawMainCityDistricts = new Collection<CityDistrictEntity>
        {
            new CityDistrictEntity
            {
                Id = FabrycznaId,
                Name = "Fabryczna",
                PolishName = "Fabryczna",
                CityId = CityEntitySeeder.WroclawId
            },
            new CityDistrictEntity
            {
                Id = KrzykiId,
                Name = "Krzyki",
                PolishName = "Krzyki",
                CityId = CityEntitySeeder.WroclawId
            },
            new CityDistrictEntity
            {
                Id = PsiePoleId,
                Name = "Psie-Pole",
                PolishName = "Psie Pole",
                CityId = CityEntitySeeder.WroclawId
            },
            new CityDistrictEntity
            {
                Id = StareMiastoId,
                Name = "Stare-Miasto",
                PolishName = "Stare Miasto",
                CityId = CityEntitySeeder.WroclawId
            },
            new CityDistrictEntity
            {
                Id = SrodmiescieId,
                Name = "Srodmiescie",
                PolishName = "Śródmieście",
                CityId = CityEntitySeeder.WroclawId
            }
        };

        private static readonly IEnumerable<CityDistrictEntity> WroclawAdministrativeCityDistricts = new Collection<CityDistrictEntity>
            {
                new CityDistrictEntity
                {
                    Id =  GajowiceId,
                    Name = "Gajowice",
                    PolishName = "Gajowice",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = GadowPopowicePoludnioweId,
                    Name = "Gadow-Popowice-Poludniowe",
                    PolishName = "Gądów-Popowice Południowe",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = GrabiszynGrabiszynekId,
                    Name = "Grabiszyn-Grabiszynek",
                    PolishName = "Grabiszyn-Grabiszynek",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = JerzmanowoJarnoltowStrachowiceOsiniecId,
                    Name = "Jerzmanowo-Jarnoltow-Strachowice-Osiniec",
                    PolishName = "Jerzmanowo-Jarnołtów-Strachowice-Osiniec",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KuznikiId,
                    Name = "Kuzniki",
                    PolishName = "Kuźniki",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = LesnicaId,
                    Name = "Lesnica",
                    PolishName = "Leśnica",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = MasliceId,
                    Name = "Maslice",
                    PolishName = "Maślice",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = MuchoborMalyId,
                    Name = "Muchobor-Maly",
                    PolishName = "Muchobór Mały",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = MuchoborWielkiId,
                    Name = "Muchobor-Wielki",
                    PolishName = "Muchobór Wielki",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = NowyDworId,
                    Name = "Nowy-Dwor",
                    PolishName = "Nowy Dwór",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("66f68e3a-50cf-49cd-8191-e1fc627ea2e4"),
                    Name = "Oporow",
                    PolishName = "Oporów",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PilczyceKozanowPopowicePolnocneId,
                    Name = "Pilczyce-Kozanow-Popowice-Polnocne",
                    PolishName = "Pilczyce-Kozanów-Popowice Północne",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PraczeOdrzanskieId,
                    Name = "Pracze-Odrzanskie",
                    PolishName = "Pracze Odrzańskie",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZernikiId,
                    Name = "Zerniki",
                    PolishName = "Żerniki",
                    ParentId = FabrycznaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = BienkowiceId,
                    Name = "Bienkowice",
                    PolishName = "Bieńkowice",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = BorekId,
                    Name = "Borek",
                    PolishName = "Borek",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("a25aa6e2-70dd-413f-9b00-949042b14f97"),
                    Name = "Brochow",
                    PolishName = "Brochów",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("69c05563-4af9-4552-b43d-b742dc33a91f"),
                    Name = "Gaj",
                    PolishName = "Gaj",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = HubyId,
                    Name = "Huby",
                    PolishName = "Huby",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = JagodnoId,
                    Name = "Jagodno",
                    PolishName = "Jagodno",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KlecinaId,
                    Name = "Klecina",
                    PolishName = "Klecina",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KrzykiPartyniceId,
                    Name = "Krzyki-Partynice",
                    PolishName = "Krzyki-Partynice",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KsiezeId,
                    Name = "Ksieze",
                    PolishName = "Księże",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("25fb993b-700f-45be-8699-c09881f7a76c"),
                    Name = "Oltaszyn",
                    PolishName = "Ołtaszyn",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PowstancowSlaskichId,
                    Name = "Powstancow-Slaskich",
                    PolishName = "Powstańców Śląskich",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PrzedmiescieOlawskieId,
                    Name = "Przedmiescie-Olawskie",
                    PolishName = "Przedmieście Oławskie",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("86c0e69c-1259-4245-b94f-9c33d12aa490"),
                    Name = "Tarnogaj",
                    PolishName = "Tarnogaj",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = WojszyceId,
                    Name = "Wojszyce",
                    PolishName = "Wojszyce",
                    ParentId = KrzykiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KarlowiceRozankaId,
                    Name = "Karlowice-Rozanka",
                    PolishName = "Karłowice-Różanka",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("29b9a280-a2a5-4436-810c-78df10740360"),
                    Name = "Kleczkow",
                    PolishName = "Kleczków",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KowaleId,
                    Name = "Kowale",
                    PolishName = "Kowale",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = LipaPiotrowskaId,
                    Name = "Lipa-Piotrowska",
                    PolishName = "Lipa Piotrowska",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = OsobowiceRedzinId,
                    Name = "Osobowice-Redzin",
                    PolishName = "Osobowice-Rędzin",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PawlowiceId,
                    Name = "Pawlowice",
                    PolishName = "Pawłowice",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PolanowicePoswietneLigotaId,
                    Name = "Polanowice-Poswietne-Ligota",
                    PolishName = "Polanowice-Poświętne-Ligota",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PsiePoleZawidawieId,
                    Name = "Psie-Pole-Zawidawie",
                    PolishName = "Psie Pole Zawidawie",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SoltysowiceId,
                    Name = "Soltysowice",
                    PolishName = "Sołtysowice",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SwojczyceStrachocinWojnowId,
                    Name = "Swojczyce-Strachocin-Wojnow",
                    PolishName = "Swojczyce-Strachocin-Wojnów",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SwiniaryId,
                    Name = "Swiniary",
                    PolishName = "Świniary",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = WidawaId,
                    Name = "Widawa",
                    PolishName = "Widawa",
                    ParentId = PsiePoleId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PrzedmiescieSwidnickieId,
                    Name = "Przedmiescie-Swidnickie",
                    PolishName = "Przedmieście Świdnickie",
                    ParentId = StareMiastoId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("ceb9a82f-0141-49ad-9927-8aeae16da244"),
                    Name = "Szczepin",
                    PolishName = "Szczepin",
                    ParentId = StareMiastoId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = BiskupinSepolnoDabieBartoszowiceId,
                    Name = "Biskupin-Sepolno-Dabie-Bartoszowice",
                    PolishName = "Biskupin-Sępolno-Dąbie-Bartoszowice",
                    ParentId = SrodmiescieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = NadodrzeId,
                    Name = "Nadodrze",
                    PolishName = "Nadodrze",
                    ParentId = SrodmiescieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("1c2ef079-52ef-4568-a06e-f882dcf28c23"),
                    Name = "Olbin",
                    PolishName = "Ołbin",
                    ParentId = SrodmiescieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id =  PlacGrunwaldzkiId,
                    Name = "Plac-Grunwaldzki",
                    PolishName = "Plac Grunwaldzki",
                    ParentId = SrodmiescieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZaciszeZalesieSzczytnikiId,
                    Name = "Zacisze-Zalesie-Szczytniki",
                    PolishName = "Zacisze-Zalesie-Szczytniki",
                    ParentId = SrodmiescieId,
                    CityId = CityEntitySeeder.WroclawId
                }
            };

        private static readonly IEnumerable<CityDistrictEntity> WroclawCityHousingEstates = new Collection<CityDistrictEntity>
            {
                new CityDistrictEntity
                {
                    Id = GadowMalyId,
                    Name = "Gadow-Maly",
                    PolishName = "Gądów Mały",
                    ParentId = GadowPopowicePoludnioweId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PopowicePoludnioweId,
                    Name = "Popowice",
                    PolishName = "Popowice",
                    ParentId = GadowPopowicePoludnioweId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = GrabiszynId,
                    Name = "Grabiszyn",
                    PolishName = "Grabiszyn",
                    ParentId = GrabiszynGrabiszynekId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = GrabiszynekId,
                    Name = "Grabiszynek",
                    PolishName = "Grabiszynek",
                    ParentId = GrabiszynGrabiszynekId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = JerzmanowoId,
                    Name = "Jerzmanowo",
                    PolishName = "Jerzmanowo",
                    ParentId = JerzmanowoJarnoltowStrachowiceOsiniecId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = JarnoltowId,
                    Name = "Jarnoltow",
                    PolishName = "Jarnołtów",
                    ParentId = JerzmanowoJarnoltowStrachowiceOsiniecId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = StrachowiceId,
                    Name = "Strachowice",
                    PolishName = "Strachowice",
                    ParentId = JerzmanowoJarnoltowStrachowiceOsiniecId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = OsiniecId,
                    Name = "Osiniec",
                    PolishName = "Osiniec",
                    ParentId = JerzmanowoJarnoltowStrachowiceOsiniecId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = StablowiceId,
                    Name = "Stablowice",
                    PolishName = "Stabłowice",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZlotnikiId,
                    Name = "Zlotniki",
                    PolishName = "Złotniki",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = MarszowiceId,
                    Name = "Marszowice",
                    PolishName = "Marszowice",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = RatynId,
                    Name = "Ratyn",
                    PolishName = "Ratyń",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = MokraId,
                    Name = "Mokra",
                    PolishName = "Mokra",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PustkiId,
                    Name = "Pustki",
                    PolishName = "Pustki",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("93dd35eb-f541-4b84-9405-5bbe23c94f2d"),
                    Name = "Zar",
                    PolishName = "Żar",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = GajowaId,
                    Name = "Gajowa",
                    PolishName = "Gajowa",
                    ParentId = LesnicaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PilczyceId,
                    Name = "Pilczyce",
                    PolishName = "Pilczyce",
                    ParentId = PilczyceKozanowPopowicePolnocneId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("7ff08f6a-cfdb-40d2-b57f-0c96288b8d7a"),
                    Name = "Kozanow",
                    PolishName = "Kozanów",
                    ParentId = PilczyceKozanowPopowicePolnocneId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PopowicePolnoceId,
                    Name = "Popowice",
                    PolishName = "Popowice",
                    ParentId = PilczyceKozanowPopowicePolnocneId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = JanowekId,
                    Name = "Janowek",
                    PolishName = "Janówek",
                    ParentId = PraczeOdrzanskieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = NowaKarczmaId,
                    Name = "Nowa-Karczma",
                    PolishName = "Nowa Karczma",
                    ParentId = PraczeOdrzanskieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = GliniankiId,
                    Name = "Glinianki",
                    PolishName = "Glinianki",
                    ParentId = HubyId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PartyniceId,
                    Name = "Partynice",
                    PolishName = "Partynice",
                    ParentId = KrzykiPartyniceId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KsiezeMaleId,
                    Name = "Ksieze-Male",
                    PolishName = "Księże Małe",
                    ParentId = KsiezeId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KsiezeWielkieId,
                    Name = "Ksieze-Wielkie",
                    PolishName = "Księże Wielkie",
                    ParentId = KsiezeId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SwiatnikiId,
                    Name = "Swiatniki",
                    PolishName = "Świątniki",
                    ParentId = KsiezeId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = OpatowiceId,
                    Name = "Opatowice",
                    PolishName = "Opatowice",
                    ParentId = KsiezeId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = BierdzanyId,
                    Name = "Bierdzany",
                    PolishName = "Bierdzany",
                    ParentId = KsiezeId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = NowyDomId,
                    Name = "Nowy-Dom",
                    PolishName = "Nowy Dom",
                    ParentId = KsiezeId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PoludnieId,
                    Name = "Poludnie",
                    PolishName = "Południe",
                    ParentId = PowstancowSlaskichId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = DworekId,
                    Name = "Dworek",
                    PolishName = "Dworek",
                    ParentId = PowstancowSlaskichId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KarlowiceId,
                    Name = "Karlowice",
                    PolishName = "Karłowice",
                    ParentId = KarlowiceRozankaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = RozankaId,
                    Name = "Rozanka",
                    PolishName = "Różanka",
                    ParentId = KarlowiceRozankaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = MirowiecId,
                    Name = "Mirowiec",
                    PolishName = "Mirowiec",
                    ParentId = KarlowiceRozankaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PolankaId,
                    Name = "Polanka",
                    PolishName = "Polanka",
                    ParentId = KarlowiceRozankaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = OsobowiceId,
                    Name = "Osobowice",
                    PolishName = "Osobowice",
                    ParentId = OsobowiceRedzinId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("02639180-bbb6-4e94-9ac6-797e4992d537"),
                    Name = "Redzin",
                    PolishName = "Rędzin",
                    ParentId = OsobowiceRedzinId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = LigotaId,
                    Name = "Ligota",
                    PolishName = "Ligota",
                    ParentId = PolanowicePoswietneLigotaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PolanowiceId,
                    Name = "Polanowice",
                    PolishName = "Polanowice",
                    ParentId = PolanowicePoswietneLigotaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PoswietneId,
                    Name = "Poswietne",
                    PolishName = "Poświętne",
                    ParentId = PolanowicePoswietneLigotaId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZakrzowId,
                    Name = "Zakrzow",
                    PolishName = "Zakrzów",
                    ParentId = PsiePoleZawidawieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZgorzeliskoId,
                    Name = "Zgorzelisko",
                    PolishName = "Zgorzelisko",
                    ParentId = PsiePoleZawidawieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = KlokoczyceId,
                    Name = "Klokoczyce",
                    PolishName = "Kłokoczyce",
                    ParentId = PsiePoleZawidawieId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = StrachocinId,
                    Name = "Strachocin",
                    PolishName = "Strachocin",
                    ParentId = SwojczyceStrachocinWojnowId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SwojczyceId,
                    Name = "Swojczyce",
                    PolishName = "Swojczyce",
                    ParentId = SwojczyceStrachocinWojnowId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = WojnowId,
                    Name = "Wojnow",
                    PolishName = "Wojnów",
                    ParentId = SwojczyceStrachocinWojnowId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = PopieleId,
                    Name = "Popiele",
                    PolishName = "Popiele",
                    ParentId = SwojczyceStrachocinWojnowId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = BartoszowiceId,
                    Name = "Bartoszowice",
                    PolishName = "Bartoszowice",
                    ParentId = BiskupinSepolnoDabieBartoszowiceId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = new Guid("b2244039-7b02-4221-bfa5-e58880238cdb"),
                    Name = "Biskupin",
                    PolishName = "Biskupin",
                    ParentId = BiskupinSepolnoDabieBartoszowiceId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = DabieId,
                    Name = "Dabie",
                    PolishName = "Dąbie",
                    ParentId = BiskupinSepolnoDabieBartoszowiceId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SepolnoId,
                    Name = "Sepolno",
                    PolishName = "Sępolno",
                    ParentId = BiskupinSepolnoDabieBartoszowiceId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = SzczytnikiId,
                    Name = "Szczytniki",
                    PolishName = "Szczytniki",
                    ParentId = ZaciszeZalesieSzczytnikiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZaciszeId,
                    Name = "Zacisze",
                    PolishName = "Zacisze",
                    ParentId = ZaciszeZalesieSzczytnikiId,
                    CityId = CityEntitySeeder.WroclawId
                },
                new CityDistrictEntity
                {
                    Id = ZalesieId,
                    Name = "Zalesie",
                    PolishName = "Zalesie",
                    ParentId = ZaciszeZalesieSzczytnikiId,
                    CityId = CityEntitySeeder.WroclawId
                }
            };

        public static IEnumerable<CityDistrictEntity> WroclawCityDistrictEntities
        {
            get
            {
                var cityDistricts = new List<CityDistrictEntity>(WroclawMainCityDistricts);
                cityDistricts.AddRange(WroclawAdministrativeCityDistricts);
                cityDistricts.AddRange(WroclawCityHousingEstates);
                return cityDistricts;
            }
        }
    }
}
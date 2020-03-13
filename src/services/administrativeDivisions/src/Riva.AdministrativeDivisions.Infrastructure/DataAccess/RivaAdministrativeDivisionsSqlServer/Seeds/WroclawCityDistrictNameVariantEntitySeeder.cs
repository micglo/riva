using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds
{
    public static class WroclawCityDistrictNameVariantEntitySeeder
    {
        private static readonly IEnumerable<CityDistrictNameVariantEntity> MainCityDistrictNameVariants =
            new Collection<CityDistrictNameVariantEntity>
            {
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("66d75b75-f425-49d2-87bd-bd109aca8a6d"),
                    Value = "Fabrycznej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.FabrycznaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("48ba02dc-da6d-4db3-a2b8-6838d658d1e2"),
                    Value = "Krzykach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KrzykiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("d1c5265d-4c56-448a-bdbd-434ab0b105c0"),
                    Value = "Psim polu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PsiePoleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("2818fd91-86cc-44ab-91e9-72e90449457f"),
                    Value = "Starym miescie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.StareMiastoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("e39a663c-5144-4ed7-a56a-dfe6569a3362"),
                    Value = "Starym mieście",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.StareMiastoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0506708d-1928-47d9-84ac-cae51292069c"),
                    Value = "Srodmiesciu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SrodmiescieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("13f00177-c102-452e-a400-dff52e62960e"),
                    Value = "Śródmieściu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SrodmiescieId
                }
            };

        private static readonly IEnumerable<CityDistrictNameVariantEntity> AdministrativeCityDistrictNameVariants =
            new Collection<CityDistrictNameVariantEntity>
            {
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c1508b6a-185b-4220-84b6-e2a2e6a20a44"),
                    Value = "Gajowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GajowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("9887aa49-d6ba-4ae8-9a02-a68ed4adfaae"),
                    Value = "Kuznikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KuznikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("6664771d-f04c-49a6-8f17-99942bd7ca88"),
                    Value = "Kuźnikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KuznikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8143a298-10ad-4930-8cb3-ca56b1f648c0"),
                    Value = "Lesnicy",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.LesnicaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("00647eee-a5a5-4756-81d8-fa1747055e98"),
                    Value = "Leśnicy",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.LesnicaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0b7bee7e-61d6-457e-9553-3640b3bd86c3"),
                    Value = "Maslicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MasliceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("031211be-eb65-4e62-8c8c-e457dcaf014e"),
                    Value = "Maślicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MasliceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("06d32d31-64fa-4df1-9817-67fbc7686775"),
                    Value = "Muchobor",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MuchoborMalyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("69b7af3b-0993-4d4b-9cfc-5081c80fd9bc"),
                    Value = "Muchobór",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MuchoborMalyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("e10eec4e-d540-4fd5-b278-e57d3d39feeb"),
                    Value = "Muchobor",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MuchoborWielkiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("a3dbdf7a-dec2-4a7b-892b-0853a5fde42b"),
                    Value = "Muchobór",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MuchoborWielkiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("36876439-f286-42d9-8eba-857629dcee60"),
                    Value = "Nowym dworze",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.NowyDworId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("cae1802d-4284-436b-a596-4b7d8a21545d"),
                    Value = "Nowy dwor",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.NowyDworId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("36f80b74-e20d-4259-858a-f2b93968e225"),
                    Value = "Praczu Odrzanskim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PraczeOdrzanskieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("79f208ef-ad26-4d97-b5ed-b8fb27af6d70"),
                    Value = "Praczu Odrzańskim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PraczeOdrzanskieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("14bf2192-8222-41bd-830f-edcd5dc1cd2f"),
                    Value = "Zernikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZernikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("5566cab4-b65c-4e46-9f79-ec340c3d4912"),
                    Value = "Żernikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZernikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("de46a69d-06c5-450c-8897-f7f4caf216ca"),
                    Value = "Bienkowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.BienkowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("d91def1f-d742-46b6-bf5d-d4a69eb5854d"),
                    Value = "Bieńkowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.BienkowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0aaa7369-4cbc-4dfc-b5fb-49a5da711793"),
                    Value = "Borku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.BorekId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("74cbad72-cc4d-49ca-bb38-2dc5e3cb63ac"),
                    Value = "Hubach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.HubyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("e1e41732-480f-4b65-b3c1-18a71de09ec1"),
                    Value = "Jagodnie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.JagodnoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("5b0c2ff8-1492-4fb6-97ea-ba9d0b502ff5"),
                    Value = "Klecinie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KlecinaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("f2b3de61-5df5-477f-ad14-b3e0e11a1adc"),
                    Value = "Ksiezach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("f97797c4-93a3-4793-9fb6-d909de3d7d30"),
                    Value = "Księżach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("fcc80916-8945-4b95-9217-566da6904d49"),
                    Value = "Powstancow slaskich",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PowstancowSlaskichId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("398c9bd5-dd85-438c-9715-107372d2f301"),
                    Value = "Przedmiesciu olawskim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PowstancowSlaskichId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("75b9425a-4be8-4353-b9c3-9562454e56a2"),
                    Value = "Przedmieściu oławskim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PrzedmiescieOlawskieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8d1c7a31-1459-42ff-a056-8ef428f9eacf"),
                    Value = "Wojszycach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.WojszyceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("adc28067-787f-4b88-9aa4-f613a503237d"),
                    Value = "Kowalach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KowaleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8553f3fb-8859-4299-b0cc-a4dc8b38af51"),
                    Value = "Lipie piotrowskiej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.LipaPiotrowskaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("2fe7c271-1988-436e-bd5e-c3caaaa28155"),
                    Value = "Pawlowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PawlowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("07e84e5a-3a55-4c12-a880-5bd56427c725"),
                    Value = "Pawłowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PawlowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("1a40665c-9f74-4082-ab12-37cf1159a17c"),
                    Value = "Soltysowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SoltysowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c50193a4-a54b-4d14-8187-6b748b436818"),
                    Value = "Sołtysowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SoltysowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("bf4a74ab-550c-4ecb-9ba7-40faf3fce1e2"),
                    Value = "Swiniarach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SwiniaryId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("1362264a-2684-493c-8115-79a400c3454f"),
                    Value = "Świniarach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SwiniaryId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("33e50a7d-a7b4-4941-8c25-c922639b007d"),
                    Value = "Widawie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.WidawaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("9d672522-5fdf-42c1-b1e2-564d76975d3a"),
                    Value = "Przedmiesciu swidnickim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PrzedmiescieSwidnickieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("038c7e67-924c-4eae-9ed7-ad6e4aa5f77d"),
                    Value = "Przedmieściu świdnickim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PrzedmiescieSwidnickieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("f08a8949-ac74-41ca-bf74-ce62f3b5c684"),
                    Value = "Nadodrzu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.NadodrzeId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("f70d50cd-f0f9-40c1-8417-c895ae78d0dd"),
                    Value = "Placu grunwaldzkim",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PlacGrunwaldzkiId
                }
            };

        private static readonly IEnumerable<CityDistrictNameVariantEntity> CityHousingEstateNameVariants =
            new Collection<CityDistrictNameVariantEntity>
            {
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("ce9dc855-4083-467e-98fa-45d5b8067bee"),
                    Value = "Gadowie malym",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GadowMalyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("fd6f22bc-a510-4795-b02f-2b33ffa84bd1"),
                    Value = "Gadowie małym",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GadowMalyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("fa2ab82d-7261-4d49-b05e-8164018bdb09"),
                    Value = "Gadowie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GadowMalyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("555faae5-c3ea-46c2-a312-3828b0cd05cf"),
                    Value = "Gądowie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GadowMalyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("102375d5-bad8-424e-ab9d-78bc8a03de5e"),
                    Value = "Popowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PopowicePoludnioweId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c97df31e-3a4e-4489-94b5-18cf7fe09b34"),
                    Value = "Grabiszynie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GrabiszynId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("3d306915-b766-420a-8993-82bbb779718d"),
                    Value = "Grabiszynku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GrabiszynekId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8dc8b068-bb87-4b46-809b-a45781f58f96"),
                    Value = "Jerzmanowej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.JerzmanowoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("4d4a90d6-773b-4e84-a0f6-07f55cff2f61"),
                    Value = "Jarnoltowku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.JarnoltowId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("a9d5e393-27d8-4d24-b008-30470d46535e"),
                    Value = "Jarnołtówku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.JarnoltowId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c59474af-af99-4ce2-8ee1-8b0dbf3eb1f0"),
                    Value = "Strachowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.StrachowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("95b5989f-8a0f-4b19-912b-b024b9c919d3"),
                    Value = "Osincu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.OsiniecId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("a5786af0-e2fd-4799-a008-0f15f512b66d"),
                    Value = "Osińcu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.OsiniecId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("e49553df-fd18-4191-8a81-4507163bb5d8"),
                    Value = "Stablowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.StablowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("e8da6925-f255-478f-b819-9868a547c995"),
                    Value = "Stabłowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.StablowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("24a63bab-fad8-4bf8-a597-cf42be71ac22"),
                    Value = "Zlotnikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZlotnikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("beafde37-3210-4660-9741-521e82b1783b"),
                    Value = "Złotnikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZlotnikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("957868bc-3233-453b-a543-768889c19e95"),
                    Value = "Marszowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MarszowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("cec42b5e-3948-454d-92e4-449fcc02cf1e"),
                    Value = "Ratuniu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.RatynId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8862266f-3ab4-4d52-b37a-27a59f9d99f7"),
                    Value = "Mokrej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MokraId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0b8cff6a-ebc7-4ced-a7b5-fa4ae46df7d3"),
                    Value = "Pustkach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PustkiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("613ea767-69f2-47db-bbe9-4e6b5387587d"),
                    Value = "Gajowej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GajowaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0a8d4b73-65dc-4b62-b350-d738bf4e7f19"),
                    Value = "Pilczycach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PilczyceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("a424b69a-e8c3-47ac-ac7e-4e72a47d8471"),
                    Value = "Popowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PopowicePolnoceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("45dfb37b-fbb4-4e5d-87b6-fed3935319d2"),
                    Value = "Janowku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.JanowekId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("87e02521-d428-4e7c-804f-cd0dfa08aa34"),
                    Value = "Janówku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.JanowekId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("6d5ce240-f42a-464c-937e-b5f20551ceea"),
                    Value = "Nowej karczmie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.NowaKarczmaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("a2100738-eb14-47e0-9196-cb69461412e7"),
                    Value = "Gliniankach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.GliniankiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("22b24ff3-3f3c-438e-a1a9-cf3eded4650a"),
                    Value = "Partynicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PartyniceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0a0421bd-f92e-40ec-aa81-654d9c796e38"),
                    Value = "Ksiezach malych",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeMaleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("555cb670-935c-4bbd-b773-5f25271103ec"),
                    Value = "Księżach małych",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeMaleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("4fbfa51b-c049-45fd-88e8-45fd03cdd819"),
                    Value = "Ksiezach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeMaleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0fba6ccd-b37a-4359-a94c-c169b8b2af8d"),
                    Value = "Księżach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeMaleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("b3d999b4-1031-48bf-9b77-847615bad75a"),
                    Value = "Ksiezach wielkich",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeWielkieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0f938c04-e8bf-4af2-9895-608c90a11286"),
                    Value = "Księżach wielkich",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeWielkieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("7f35c1d1-e12a-4f78-b976-adfc3efb10c2"),
                    Value = "Ksiezach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeWielkieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("2aa68cd4-36aa-430f-b48d-cb147627c401"),
                    Value = "Księżach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KsiezeWielkieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("ee076fa3-7401-418e-9fe0-a22d16953535"),
                    Value = "Swiatnikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SwiatnikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("12346d4c-6726-4531-a24f-ddfbef4abcc3"),
                    Value = "Świątnikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SwiatnikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("5cc8a418-24b3-48ea-9438-b7c51818b84f"),
                    Value = "Opatowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.OpatowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("19b50d37-d315-45cc-873f-d6b0e7d51c59"),
                    Value = "Bierdzanach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.BierdzanyId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("0651439c-bf20-45d2-9ceb-ee5860a036cd"),
                    Value = "Nowym domie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.NowyDomId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("fa14c562-e3c8-4a0f-ac77-2b54c7a9567f"),
                    Value = "Poludniu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PoludnieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("27ca7cbe-07c4-46f8-b6a3-9c6583121c0e"),
                    Value = "Południu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PoludnieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("d210a28a-04ac-4f90-9697-955048e215a9"),
                    Value = "Dworku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.DworekId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("b75d510f-974f-46c9-a8e5-263deb2ec7f9"),
                    Value = "Karlowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KarlowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c7024bc0-706e-4207-8ed3-458b6ae0a5b0"),
                    Value = "Karłowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KarlowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("cef3a3e1-7b49-42a6-89ad-a63b35e4cb24"),
                    Value = "Rozance",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.RozankaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8de4117c-6c9e-4ff8-a207-ae26e8a98b47"),
                    Value = "Różance",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.RozankaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("4183882f-72ca-4d0c-a28e-eaede21d9ea3"),
                    Value = "Mirowcu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.MirowiecId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("a828a718-9827-47f2-bd1e-9bd7df8b03a5"),
                    Value = "Polance",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PolankaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c9d79cef-309e-422a-825f-cf1a02f8e31d"),
                    Value = "Ligocie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.LigotaId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("12a9bc9a-4207-4d0e-b1db-967050d4cdd7"),
                    Value = "Polanowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PolanowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("40ac09e2-cf6b-409f-b615-0e9436ae298d"),
                    Value = "Poswietnym",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PoswietneId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("841c651c-fb9a-401e-815f-cd6ab7bca735"),
                    Value = "Poświętnym",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PoswietneId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("5bf332e3-2d61-485b-8bb4-d7f8e3f94fc3"),
                    Value = "Poswietnej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PoswietneId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("b8223315-487f-42dd-a0e4-079f8a9f9ed5"),
                    Value = "Poświętnej",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PoswietneId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("720e49e0-7d45-4b31-9cbd-ee99ea23944e"),
                    Value = "Zakrzowie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZakrzowId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("18a0f2a3-4a9f-4d9c-8e54-a468f73e3016"),
                    Value = "Zgorzelisku",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZgorzeliskoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("1b49dee5-b31f-42a4-b684-4ff375852958"),
                    Value = "Klokoczycach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KlokoczyceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("66beb497-5b50-4555-b506-6da578d529cc"),
                    Value = "Kłokoczycach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.KlokoczyceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("c9105703-487c-4ac0-879f-f6c7f83c1274"),
                    Value = "Swojszycach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SwojczyceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("55e05adc-8f32-405d-810e-196eb539cd6a"),
                    Value = "Wojnowie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.WojnowId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("1152ab6d-77ad-4923-9712-d56d4c0c307d"),
                    Value = "Popielach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.PopieleId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("e31e1cd0-f62b-495c-a51f-6d1afaeff0d5"),
                    Value = "Bartoszowicach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.BartoszowiceId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("2492d889-2847-4e9e-8983-d88e2ae9a388"),
                    Value = "Dabiach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.DabieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("cc7060d7-6402-4211-887c-d774c2880534"),
                    Value = "Dąbiach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.DabieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("76c5dafb-9e64-4bd1-84f3-afd77f63a394"),
                    Value = "Dabiu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.DabieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("1245afd8-097f-4270-b123-cd6c49c4b005"),
                    Value = "Dąbiu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.DabieId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("af49739d-41ae-4c00-a285-5ebea27c1580"),
                    Value = "Sepolnie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SepolnoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("9d4f921e-d7e2-4850-9130-9755688c17cf"),
                    Value = "Sępolnie",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SepolnoId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("efae3434-18c0-4522-8e73-78af7e558156"),
                    Value = "Szczytnikach",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.SzczytnikiId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("8d09ca52-81f3-4081-a9a6-b0a38f061054"),
                    Value = "Zaciszu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZaciszeId
                },
                new CityDistrictNameVariantEntity
                {
                    Id = new Guid("68524e8d-dac4-449d-bab5-587ce356b222"),
                    Value = "Zalesiu",
                    CityDistrictId = WroclawCityDistrictEntitySeeder.ZalesieId
                }
            };

        public static IEnumerable<CityDistrictNameVariantEntity> WroclawCityDistrictNameVariantEntities
        {
            get
            {
                var nameVariants = new List<CityDistrictNameVariantEntity>(MainCityDistrictNameVariants);
                nameVariants.AddRange(AdministrativeCityDistrictNameVariants);
                nameVariants.AddRange(CityHousingEstateNameVariants);
                return nameVariants;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds
{
    public static class StateEntitySeeder
    {
        public static Guid DolnoslaskieId = new Guid("23cbd1a9-78b5-4b7f-82f8-510e792d6022");

        public static IEnumerable<StateEntity> StateEntities = new Collection<StateEntity>
        {
            new StateEntity
            {
                Id = DolnoslaskieId,
                Name = "Dolnoslaskie",
                PolishName = "Dolnośląskie"
            },
            new StateEntity
            {
                Id = new Guid("0b2d0ba0-0365-4d6a-a2bf-f05a9fd08871"),
                Name = "Kujawsko-pomorskie",
                PolishName = "Kujawsko-pomorskie"
            },
            new StateEntity
            {
                Id = new Guid("9d2640f7-266e-4274-9f42-a9cd14aa41e1"),
                Name = "Lubelskie",
                PolishName = "Lubelskie"
            },
            new StateEntity
            {
                Id = new Guid("c2c3864c-7d24-4f26-a286-9e859352e231"),
                Name = "Lubuskie",
                PolishName = "Lubuskie"
            },
            new StateEntity
            {
                Id = new Guid("b2d6b891-65a3-4b8e-8a15-f80b335d4e1a"),
                Name = "Lodzkie",
                PolishName = "Łódzkie"
            },
            new StateEntity
            {
                Id = new Guid("f5168cc9-f98d-4a4e-a4d9-c3fc120d1987"),
                Name = "Malopolskie",
                PolishName = "Małopolskie"
            },
            new StateEntity
            {
                Id = new Guid("008e4495-51ad-4c2c-ab97-6f93f4aaa42e"),
                Name = "Mazowieckie",
                PolishName = "Mazowieckie"
            },
            new StateEntity
            {
                Id = new Guid("c90a386f-1866-4792-8bb6-b2f4b244126c"),
                Name = "Opolskie",
                PolishName = "Opolskie"
            },
            new StateEntity
            {
                Id = new Guid("59fd3598-a69e-47aa-8f0b-09714f605267"),
                Name = "Podkarpackie",
                PolishName = "Podkarpackie"
            },
            new StateEntity
            {
                Id = new Guid("ac1a4a8e-fd7a-4b00-a3f0-9a6a9d071497"),
                Name = "Podlaskie",
                PolishName = "Podlaskie"
            },
            new StateEntity
            {
                Id = new Guid("6712c6ae-b8cf-4447-9d63-37f980d5af83"),
                Name = "Pomorskie",
                PolishName = "Pomorskie"
            },
            new StateEntity
            {
                Id = new Guid("c44814d9-357b-4bc0-b4a2-25d056e1b448"),
                Name = "Slaskie",
                PolishName = "Śląskie"
            },
            new StateEntity
            {
                Id = new Guid("d22ea8e8-0f4d-4f1d-b04f-15ae2e58cb35"),
                Name = "Swietokrzyskie",
                PolishName = "Świętokrzyskie"
            },
            new StateEntity
            {
                Id = new Guid("dbc5ecba-6560-40cd-8639-1ca31a606b01"),
                Name = "Warminsko-mazurskie",
                PolishName = "Warmińsko-mazurskie"
            },
            new StateEntity
            {
                Id = new Guid("b6927901-a3e7-44be-bcfc-0f01e669dc7f"),
                Name = "Wielkopolskie",
                PolishName = "Wielkopolskie"
            },
            new StateEntity
            {
                Id = new Guid("baadea02-7671-454e-8cc0-dc5b8e71af5d"),
                Name = "Zachodniopomorskie",
                PolishName = "Zachodniopomorskie"
            }
        };
    }
}
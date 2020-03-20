using System;
using System.Net.Http;
using Cosmonaut;
using Microsoft.AspNetCore.Mvc.Testing;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
{
    public class IntegrationTestFixture : IDisposable
    {
        private readonly AdministratorWebApplicationFactory _administratorFactory;
        private readonly UserWebApplicationFactory _userFactory;
        private readonly AnonymousWebApplicationFactory _anonymousFactory;

        public HttpClient AdministratorHttpClient { get; }

        public HttpClient UserHttpClient { get; }

        public HttpClient AnonymousHttpClient { get; }

        public ICosmosStore<FlatForRentAnnouncementEntity> FlatForRentAnnouncementEntityCosmosStore { get; }

        public ICosmosStore<RoomForRentAnnouncementEntity> RoomForRentAnnouncementEntityCosmosStore { get; }

        public IntegrationTestFixture()
        {
            _administratorFactory = new AdministratorWebApplicationFactory();
            _userFactory = new UserWebApplicationFactory();
            _anonymousFactory = new AnonymousWebApplicationFactory();

            AdministratorHttpClient = _administratorFactory
                .WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            UserHttpClient = _userFactory
                .WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            AnonymousHttpClient = _anonymousFactory
                .WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            FlatForRentAnnouncementEntityCosmosStore = _administratorFactory.FlatForRentAnnouncementEntityCosmosStore;

            RoomForRentAnnouncementEntityCosmosStore = _administratorFactory.RoomForRentAnnouncementEntityCosmosStore;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _administratorFactory?.Dispose();
                _userFactory?.Dispose();
                _anonymousFactory?.Dispose();
            }
        }
    }
}
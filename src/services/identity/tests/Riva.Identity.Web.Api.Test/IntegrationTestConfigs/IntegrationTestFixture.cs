using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
{
    public class IntegrationTestFixture : IDisposable
    {
        private readonly AdministratorWebApplicationFactory _administratorFactory;
        private readonly UserWebApplicationFactory _userFactory;
        private readonly AnonymousWebApplicationFactory _anonymousFactory;

        public HttpClient AdministratorHttpClient { get; }

        public HttpClient UserHttpClient { get; }

        public HttpClient AnonymousHttpClient { get; }

        public RivaIdentityDbContext AdministratorDbContext { get; }

        public RivaIdentityDbContext AccountDbContext { get; }

        public RivaIdentityDbContext AnonymousDbContext { get; }

        public IntegrationTestFixture()
        {
            _administratorFactory = new AdministratorWebApplicationFactory("RivaIdentityWebAppIntegrationTestsAdministratorDb");
            _userFactory = new UserWebApplicationFactory("RivaIdentityWebAppIntegrationTestsAccountDb");
            _anonymousFactory = new AnonymousWebApplicationFactory("RivaIdentityWebAppIntegrationTestsAnonymousDb");

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

            AdministratorDbContext = _administratorFactory.DbContext;
            AccountDbContext = _userFactory.DbContext;
            AnonymousDbContext = _anonymousFactory.DbContext;
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
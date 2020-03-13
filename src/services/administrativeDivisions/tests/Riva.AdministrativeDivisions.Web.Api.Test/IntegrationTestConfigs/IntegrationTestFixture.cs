using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs
{
    public class IntegrationTestFixture : IDisposable
    {
        private readonly AdministratorWebApplicationFactory _administratorFactory;
        private readonly UserWebApplicationFactory _userFactory;
        private readonly AnonymousWebApplicationFactory _anonymousFactory;

        public HttpClient AdministratorHttpClient { get; }

        public HttpClient UserHttpClient { get; }

        public HttpClient AnonymousHttpClient { get; }

        public RivaAdministrativeDivisionsDbContext AdministratorDbContext { get; }

        public RivaAdministrativeDivisionsDbContext UserDbContext { get; }

        public RivaAdministrativeDivisionsDbContext AnonymousDbContext { get; }

        public IntegrationTestFixture()
        {
            _administratorFactory = new AdministratorWebApplicationFactory("RivaAdministrativeDivisionsWebApiIntegrationTestsAdministratorDb");
            _userFactory = new UserWebApplicationFactory("RivaAdministrativeDivisionsWebApiIntegrationTestsUserDb");
            _anonymousFactory = new AnonymousWebApplicationFactory("RivaAdministrativeDivisionsWebApiIntegrationTestsAnonymousDb");

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
            UserDbContext = _userFactory.DbContext;
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
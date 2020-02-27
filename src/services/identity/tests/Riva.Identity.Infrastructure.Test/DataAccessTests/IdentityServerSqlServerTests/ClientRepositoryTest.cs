using System;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Stores;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Domain.Clients.Aggregates;
using Riva.Identity.Domain.Clients.Repositories;
using Riva.Identity.Infrastructure.DataAccess.IdentityServerSqlServer.Repositories;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.IdentityServerSqlServerTests
{
    public class ClientRepositoryTest
    {
        private readonly Mock<IClientStore> _clientStoreMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IClientRepository _clientRepository;

        public ClientRepositoryTest()
        {
            _clientStoreMock = new Mock<IClientStore>();
            _mapperMock = new Mock<IMapper>();
            _clientRepository = new ClientRepository(_clientStoreMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Client()
        {
            var clientIdentityServer = new IdentityServer4.Models.Client
            {
                ClientId = Guid.NewGuid().ToString()
            };
            var client = Client.Builder()
                .SetId(new Guid(clientIdentityServer.ClientId))
                .SetEnabled(clientIdentityServer.Enabled)
                .SetEnableLocalLogin(clientIdentityServer.EnableLocalLogin)
                .SetRequirePkce(clientIdentityServer.RequirePkce)
                .SetIdentityProviderRestrictions(clientIdentityServer.IdentityProviderRestrictions)
                .Build();

            _clientStoreMock.Setup(x => x.FindClientByIdAsync(It.IsAny<string>())).ReturnsAsync(clientIdentityServer);
            _mapperMock.Setup(x =>
                    x.Map<IdentityServer4.Models.Client, Client>(It.IsAny<IdentityServer4.Models.Client>()))
                .Returns(client);

            var result = await _clientRepository.GetByIdAsync(client.Id);
            result.Should().BeEquivalentTo(client);
        }
    }
}
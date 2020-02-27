using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;
using Xunit;
using AuthorizationRequest = Riva.Identity.Core.Models.AuthorizationRequest;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AuthorizationServiceTest
    {
        private readonly Mock<IIdentityServerInteractionService> _identityServerInteractionServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationServiceTest()
        {
            _identityServerInteractionServiceMock = new Mock<IIdentityServerInteractionService>();
            _mapperMock = new Mock<IMapper>();
            _authorizationService = new AuthorizationService(_identityServerInteractionServiceMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAuthorizationRequestAsync_Should_Return_AuthorizationRequest()
        {
            const string returnUrl = "returnUrl";
            var identityServerAuthorizationRequest = new IdentityServer4.Models.AuthorizationRequest
            {
                IdP = "IdP",
                Client = new Client
                {
                    ClientId = "clientId"
                },
                RedirectUri = "redirectUri"
            };
            var authorizationRequest = new AuthorizationRequest(identityServerAuthorizationRequest.IdP,
                identityServerAuthorizationRequest.Client.ClientId, identityServerAuthorizationRequest.RedirectUri);

            _identityServerInteractionServiceMock.Setup(x => x.GetAuthorizationContextAsync(It.IsAny<string>()))
                .ReturnsAsync(identityServerAuthorizationRequest);
            _mapperMock.Setup(x =>
                x.Map<IdentityServer4.Models.AuthorizationRequest, AuthorizationRequest>(
                    It.IsAny<IdentityServer4.Models.AuthorizationRequest>())).Returns(authorizationRequest);

            var result = await _authorizationService.GetAuthorizationRequestAsync(returnUrl);

            result.Should().BeEquivalentTo(authorizationRequest);
        }
    }
}
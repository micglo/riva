using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Services;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class LogoutServiceTest
    {
        private readonly Mock<IIdentityServerInteractionService> _identityServerInteractionServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ILogoutService _service;

        public LogoutServiceTest()
        {
            _identityServerInteractionServiceMock = new Mock<IIdentityServerInteractionService>();
            _mapperMock = new Mock<IMapper>();
            _service = new LogoutService(_identityServerInteractionServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetLogoutRequestAsync_Should_Return_LogoutRequest()
        {
            const string logoutId = "logoutId";
            var identityServerLogoutRequest = new IdentityServer4.Models.LogoutRequest("iframeUrl", new IdentityServer4.Models.LogoutMessage());
            var logoutRequest = new LogoutRequest(identityServerLogoutRequest.ShowSignoutPrompt,
                identityServerLogoutRequest.PostLogoutRedirectUri, identityServerLogoutRequest.SignOutIFrameUrl, null, null);

            _identityServerInteractionServiceMock.Setup(x => x.GetLogoutContextAsync(It.IsAny<string>()))
                .ReturnsAsync(identityServerLogoutRequest);
            _mapperMock.Setup(x =>
                x.Map<IdentityServer4.Models.LogoutRequest, LogoutRequest>(
                    It.IsAny<IdentityServer4.Models.LogoutRequest>())).Returns(logoutRequest);

            var result = await _service.GetLogoutRequestAsync(logoutId);

            result.Should().BeEquivalentTo(logoutRequest);
        }

        [Fact]
        public async Task CreateLogoutContextAsync_Should_Create_LogoutContext()
        {
            const string logoutContext = "LogoutContext";

            _identityServerInteractionServiceMock.Setup(x => x.CreateLogoutContextAsync()).ReturnsAsync(logoutContext);

            var result = await _service.CreateLogoutContextAsync();

            result.Should().Be(logoutContext);
        }
    }
}
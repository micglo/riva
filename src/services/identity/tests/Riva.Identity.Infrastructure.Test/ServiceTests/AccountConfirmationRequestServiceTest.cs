using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Models.AppSettings;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AccountConfirmationRequestServiceTest
    {
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly IAccountConfirmationRequestService _service;

        public AccountConfirmationRequestServiceTest()
        {
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            var optionsMock = new Mock<IOptions<RivaWebApplicationSetting>>();
            var rivaWebApplicationSetting = new RivaWebApplicationSetting
            {
                RivaWebRegistrationConfirmationUrl = "http://RivaWebRegistrationConfirmationUrl.com"
            };
            optionsMock.SetupGet(x => x.Value).Returns(rivaWebApplicationSetting);
            _service = new AccountConfirmationRequestService(_integrationEventBusMock.Object, optionsMock.Object);
        }

        [Fact]
        public async Task PublishAccountConfirmationRequestedIntegrationEventAsync_Should_Publish_AccountConfirmationRequestedIntegrationEvent()
        {
            const string email = "email@email.com";
            const string token = "12345";
            var correlationId = Guid.NewGuid();

            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _service.PublishAccountConfirmationRequestedIntegrationEventAsync(email, token, correlationId);
            
            await result.Should().NotThrowAsync<Exception>();
            _integrationEventBusMock.Verify(
                x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(integrationEvent =>
                    integrationEvent.CorrelationId == correlationId)), Times.Once);
        }
    }
}
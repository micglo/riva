using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Models.AppSettings;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountConfirmationRequestService : IAccountConfirmationRequestService
    {
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly string _registrationConfirmationUrl;

        public AccountConfirmationRequestService(IIntegrationEventBus integrationEventBus, IOptions<RivaWebApplicationSetting> options)
        {
            _integrationEventBus = integrationEventBus;
            _registrationConfirmationUrl = options.Value.RivaWebRegistrationConfirmationUrl;
        }

        public Task PublishAccountConfirmationRequestedIntegrationEventAsync(string email, string token, Guid correlationId)
        {
            var url = new Uri($"{_registrationConfirmationUrl}?email={email}&code={token}");
            var messageBody = $"Please use this <strong><a href={url}>link</a></strong> to confirm Your account.";

            var integrationEvent = new AccountConfirmationRequestedIntegrationEvent(correlationId, email, messageBody);
            return _integrationEventBus.PublishIntegrationEventAsync(integrationEvent);
        }
    }
}
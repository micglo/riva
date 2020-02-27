using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Models.AppSettings;

namespace Riva.Identity.Infrastructure.Services
{
    public class PasswordResetTokenRequestService : IPasswordResetTokenRequestService
    {
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly string _resetPasswordUrl;

        public PasswordResetTokenRequestService(IIntegrationEventBus integrationEventBus, IOptions<RivaWebApplicationSetting> options)
        {
            _integrationEventBus = integrationEventBus;
            _resetPasswordUrl = options.Value.RivaWebResetPasswordUrl;
        }

        public Task PublishPasswordResetRequestedIntegrationEventAsync(string email, string token, Guid correlationId)
        {
            var url = new Uri($"{_resetPasswordUrl}?email={email}&code={token}");
            var messageBody = $"Please use this <strong><a href={url}>link</a></strong> to reset Your password.";

            var integrationEvent = new PasswordResetRequestedIntegrationEvent(correlationId, email, messageBody);
            return _integrationEventBus.PublishIntegrationEventAsync(integrationEvent);
        }
    }
}
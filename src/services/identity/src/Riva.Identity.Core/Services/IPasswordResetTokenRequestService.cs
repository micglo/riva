using System;
using System.Threading.Tasks;

namespace Riva.Identity.Core.Services
{
    public interface IPasswordResetTokenRequestService
    {
        Task PublishPasswordResetRequestedIntegrationEventAsync(string email, string token, Guid correlationId);
    }
}
using System;
using System.Threading.Tasks;

namespace Riva.Identity.Core.Services
{
    public interface IAccountConfirmationRequestService
    {
        Task PublishAccountConfirmationRequestedIntegrationEventAsync(string email, string token, Guid correlationId);
    }
}
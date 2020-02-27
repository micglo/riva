using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Web.DependencyInjection
{
    public static class DomainEventHandlersRegistrar
    {
        public static IServiceCollection RegisterDomainEventHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IDomainEventHandler<AccountLoggedInDomainEvent>, AccountLoggedInDomainEventHandler>();
        }
    }
}
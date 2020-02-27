using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Web.Api.DependencyInjection
{
    public static class DomainEventHandlersRegistrar
    {
        public static IServiceCollection RegisterDomainEventHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IDomainEventHandler<AccountConfirmedDomainEvent>, AccountConfirmedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountCreatedDomainEvent>, AccountCreatedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountPasswordChangedDomainEvent>, AccountPasswordChangedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountRoleAddedDomainEvent>, AccountRoleAddedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountRoleDeletedDomainEvent>, AccountRoleDeletedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountTokenGeneratedDomainEvent>, AccountTokenGeneratedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountDeletedDomainEvent>, AccountDeletedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<AccountSecurityStampChangedDomainEvent>, AccountSecurityStampChangedDomainEventHandler>();
        }
    }
}
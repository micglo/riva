using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers;
using Riva.Users.Domain.Users.Events;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class DomainEventHandlersRegistrar
    {
        public static IServiceCollection RegisterDomainEventHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IDomainEventHandler<UserCreatedDomainEvent>, UserCreatedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserDeletedDomainEvent>, UserDeletedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserAnnouncementSendingFrequencyChangedDomainEvent>, UserAnnouncementSendingFrequencyChangedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserAnnouncementPreferenceLimitChangedDomainEvent>, UserAnnouncementPreferenceLimitChangedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserFlatForRentAnnouncementPreferenceAddedDomainEvent>, UserFlatForRentAnnouncementPreferenceAddedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserServiceActiveChangedDomainEvent>, UserServiceActiveChangedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserFlatForRentAnnouncementPreferenceChangedDomainEvent>, UserFlatForRentAnnouncementPreferenceChangedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserFlatForRentAnnouncementPreferenceDeletedDomainEvent>, UserFlatForRentAnnouncementPreferenceDeletedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserRoomForRentAnnouncementPreferenceAddedDomainEvent>, UserRoomForRentAnnouncementPreferenceAddedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserRoomForRentAnnouncementPreferenceChangedDomainEvent>, UserRoomForRentAnnouncementPreferenceChangedDomainEventHandler>()
                .AddScoped<IDomainEventHandler<UserRoomForRentAnnouncementPreferenceDeletedDomainEvent>, UserRoomForRentAnnouncementPreferenceDeletedDomainEventHandler>();
        }
    }
}
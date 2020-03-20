using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Core.Commands;
using Riva.Announcements.Core.Commands.Handlers;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Announcements.Web.Api.DependencyInjection
{
    public static class CommandHandlersRegistrar
    {
        public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandHandler<CreateRoomForRentAnnouncementCommand>,
                    CreateRoomForRentAnnouncementCommandHandler>()
                .AddScoped<ICommandHandler<DeleteRoomForRentAnnouncementCommand>,
                    DeleteRoomForRentAnnouncementCommandHandler>()
                .AddScoped<ICommandHandler<UpdateRoomForRentAnnouncementCommand>,
                    UpdateRoomForRentAnnouncementCommandHandler>()
                .AddScoped<ICommandHandler<CreateFlatForRentAnnouncementCommand>,
                    CreateFlatForRentAnnouncementCommandHandler>()
                .AddScoped<ICommandHandler<DeleteFlatForRentAnnouncementCommand>,
                    DeleteFlatForRentAnnouncementCommandHandler>()
                .AddScoped<ICommandHandler<UpdateFlatForRentAnnouncementCommand>,
                    UpdateFlatForRentAnnouncementCommandHandler>();
        }
    }
}
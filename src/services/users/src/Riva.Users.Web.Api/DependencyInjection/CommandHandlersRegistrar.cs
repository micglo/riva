using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Commands.Handlers;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class CommandHandlersRegistrar
    {
        public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandHandler<CreateUserCommand>, CreateUserCommandHandler>()
                .AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserCommandHandler>()
                .AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserCommandHandler>()
                .AddScoped<ICommandHandler<CreateFlatForRentAnnouncementPreferenceCommand>, CreateFlatForRentAnnouncementPreferenceCommandHandler>()
                .AddScoped<ICommandHandler<UpdateFlatForRentAnnouncementPreferenceCommand>, UpdateFlatForRentAnnouncementPreferenceCommandHandler>()
                .AddScoped<ICommandHandler<DeleteFlatForRentAnnouncementPreferenceCommand>, DeleteFlatForRentAnnouncementPreferenceCommandHandler>()
                .AddScoped<ICommandHandler<CreateRoomForRentAnnouncementPreferenceCommand>, CreateRoomForRentAnnouncementPreferenceCommandHandler>()
                .AddScoped<ICommandHandler<UpdateRoomForRentAnnouncementPreferenceCommand>, UpdateRoomForRentAnnouncementPreferenceCommandHandler>()
                .AddScoped<ICommandHandler<DeleteRoomForRentAnnouncementPreferenceCommand>, DeleteRoomForRentAnnouncementPreferenceCommandHandler>();
        }
    }
}
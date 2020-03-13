using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Commands.Handlers;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Web.Api.DependencyInjection
{
    public static class CommandHandlersRegistrar
    {
        public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandHandler<CreateStateCommand>, CreateStateCommandHandler>()
                .AddScoped<ICommandHandler<UpdateStateCommand>, UpdateStateCommandHandler>()
                .AddScoped<ICommandHandler<DeleteStateCommand>, DeleteStateCommandHandler>()
                .AddScoped<ICommandHandler<CreateCityCommand>, CreateCityCommandHandler>()
                .AddScoped<ICommandHandler<UpdateCityCommand>, UpdateCityCommandHandler>()
                .AddScoped<ICommandHandler<DeleteCityCommand>, DeleteCityCommandHandler>()
                .AddScoped<ICommandHandler<CreateCityDistrictCommand>, CreateCityDistrictCommandHandler>()
                .AddScoped<ICommandHandler<UpdateCityDistrictCommand>, UpdateCityDistrictCommandHandler>()
                .AddScoped<ICommandHandler<DeleteCityDistrictCommand>, DeleteCityDistrictCommandHandler>();
        }
    }
}
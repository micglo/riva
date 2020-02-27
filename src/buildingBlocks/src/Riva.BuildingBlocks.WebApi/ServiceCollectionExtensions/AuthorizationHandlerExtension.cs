using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.Authorization.AuthorizationHandlers;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class AuthorizationHandlerExtension
    {
        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
{
    public class AdministratorIntegrationTestAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AdministratorIntegrationTestAuthorizeMiddleware(RequestDelegate rd)
        {
            _next = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.User = AuthUserOptions.CreateAdministratorClaimsPrincipal(ApiResourcesConstants.RivaAnnouncementsApiResource.Name);
            await _next.Invoke(httpContext);
        }
    }

    public class UserIntegrationTestAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIntegrationTestAuthorizeMiddleware(RequestDelegate rd)
        {
            _next = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.User = AuthUserOptions.CreateUserClaimsPrincipal(ApiResourcesConstants.RivaAnnouncementsApiResource.Name);
            await _next.Invoke(httpContext);
        }
    }
}
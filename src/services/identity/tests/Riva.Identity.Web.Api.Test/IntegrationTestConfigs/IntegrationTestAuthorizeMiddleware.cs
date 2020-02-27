using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
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
            httpContext.User = AuthUserOptions.CreateAdministratorClaimsPrincipal(ApiResourcesConstants.RivaIdentityApiResource.Name);
            await _next.Invoke(httpContext);
        }
    }

    public class AccountIntegrationTestAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AccountIntegrationTestAuthorizeMiddleware(RequestDelegate rd)
        {
            _next = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.User = AuthUserOptions.CreateUserClaimsPrincipal(ApiResourcesConstants.RivaIdentityApiResource.Name);
            await _next.Invoke(httpContext);
        }
    }
}
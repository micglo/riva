using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Authorization.Requirements;

namespace Riva.BuildingBlocks.WebApi.Authorization.AuthorizationHandlers
{
    public class AdministratorSystemAuthorizationHandler : AuthorizationHandler<AdministratorSystemRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorSystemRequirement requirement)
        {
            if (context.User.IsInRole(DefaultRoleEnumeration.Administrator.DisplayName) ||
                context.User.HasClaim(c => c.Type.Equals("client_role") && c.Value.Equals(DefaultRoleEnumeration.System.DisplayName)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
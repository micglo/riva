using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Authorization.Requirements;

namespace Riva.BuildingBlocks.WebApi.Authorization.AuthorizationHandlers
{
    public class SystemAuthorizationHandler : AuthorizationHandler<SystemRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SystemRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type.Equals("client_role") && c.Value.Equals(DefaultRoleEnumeration.System.DisplayName)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
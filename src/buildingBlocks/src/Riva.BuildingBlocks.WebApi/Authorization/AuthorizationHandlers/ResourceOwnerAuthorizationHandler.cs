using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Authorization.Requirements;

namespace Riva.BuildingBlocks.WebApi.Authorization.AuthorizationHandlers
{
    public class ResourceOwnerAuthorizationHandler : AuthorizationHandler<ResourceOwnerRequirement, Guid>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOwnerRequirement requirement,
            Guid resourceId)
        {
            var nameIdentifier = new Guid(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = context.User.IsInRole(DefaultRoleEnumeration.Administrator.DisplayName);

            if (nameIdentifier == resourceId || isAdmin)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
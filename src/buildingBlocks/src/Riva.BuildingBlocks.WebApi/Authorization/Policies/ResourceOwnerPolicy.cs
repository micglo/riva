using Riva.BuildingBlocks.WebApi.Authorization.Requirements;
using Riva.BuildingBlocks.WebApi.Models.Configs;

namespace Riva.BuildingBlocks.WebApi.Authorization.Policies
{
    public class ResourceOwnerPolicy
    {
        public const string ResourceOwnerPolicyName = "ResourceOwnerPolicy";

        public static AuthorizationPolicy CreateResourceOwnerPolicy()
        {
            var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .AddRequirements(new ResourceOwnerRequirement())
                .Build();

            return new AuthorizationPolicy(ResourceOwnerPolicyName, policy);
        }
    }
}
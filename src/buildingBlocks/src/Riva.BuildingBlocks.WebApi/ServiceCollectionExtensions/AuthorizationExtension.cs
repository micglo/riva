using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Authorization.AuthorizationHandlers;
using Riva.BuildingBlocks.WebApi.Authorization.Requirements;
using Riva.BuildingBlocks.WebApi.Models.Configs;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class AuthorizationExtension
    {
        public const string JwtBearerUserPolicyName = "JwtBearerUserPolicy";
        public const string JwtBearerUserSystemPolicyName = "JwtBearerUserSystemPolicy";
        public const string JwtBearerAdministratorPolicyName = "JwtBearerAdministratorPolicy";
        public const string JwtBearerAdministratorSystemPolicyName = "JwtBearerAdministratorSystemPolicy";
        public const string JwtBearerSystemPolicyName = "JwtBearerSystemPolicy";

        public static IServiceCollection AddAuthorization(this IServiceCollection services,
            AuthorizationExtensionConfig config)
        {
            return services
                .AddSingleton<IAuthorizationHandler, UserSystemAuthorizationHandler>()
                .AddSingleton<IAuthorizationHandler, AdministratorSystemAuthorizationHandler>()
                .AddSingleton<IAuthorizationHandler, SystemAuthorizationHandler>()
                .AddAuthorization(options =>
                {
                    var policyBuilder = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope, config.ApiResourceName)
                        .AddAuthenticationSchemes(AuthenticationExtension.JwtBearerAuthenticationScheme);
                    var userPolicy = policyBuilder
                        .RequireRole(DefaultRoleEnumeration.User.DisplayName)
                        .Build();
                    var userSystemPolicy = policyBuilder
                        .AddRequirements(new UserSystemRequirement())
                        .Build();
                    var administratorPolicy = policyBuilder
                        .RequireRole(DefaultRoleEnumeration.Administrator.DisplayName)
                        .Build();
                    var administratorSystemPolicy = policyBuilder
                        .AddRequirements(new AdministratorSystemRequirement())
                        .Build();
                    var systemPolicy = policyBuilder
                        .AddRequirements(new SystemRequirement())
                        .Build();

                    options.DefaultPolicy = userPolicy;
                    options.AddPolicy(JwtBearerUserPolicyName, userPolicy);
                    options.AddPolicy(JwtBearerUserSystemPolicyName, userSystemPolicy);
                    options.AddPolicy(JwtBearerAdministratorPolicyName, administratorPolicy);
                    options.AddPolicy(JwtBearerAdministratorSystemPolicyName, administratorSystemPolicy);
                    options.AddPolicy(JwtBearerSystemPolicyName, systemPolicy);

                    foreach (var policy in config.Policies)
                    {
                        options.AddPolicy(policy.Name, policy.Policy);
                    }
                });
        }
    }
}
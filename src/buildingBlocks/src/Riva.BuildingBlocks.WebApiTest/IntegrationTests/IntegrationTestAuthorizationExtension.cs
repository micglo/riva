using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Authorization.AuthorizationHandlers;
using Riva.BuildingBlocks.WebApi.Authorization.Requirements;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;

namespace Riva.BuildingBlocks.WebApiTest.IntegrationTests
{
    public static class IntegrationTestAuthorizationExtension
    {
        public static IServiceCollection AddAuthorizationForIntegrationTest(this IServiceCollection services, AuthorizationExtensionConfig config)
        {
            return services
                .AddSingleton<IAuthorizationHandler, UserSystemAuthorizationHandler>()
                .AddSingleton<IAuthorizationHandler, AdministratorSystemAuthorizationHandler>()
                .AddAuthorization(options =>
                {
                    var policyBuilder = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(JwtClaimTypes.Scope, config.ApiResourceName);
                    var userPolicy = policyBuilder
                        .RequireRole(DefaultRoleEnumeration.User.DisplayName)
                        .Build();
                    var userSystemPolicy = policyBuilder
                        .AddRequirements(new UserSystemRequirement())
                        .Build();
                    var administratorPolicy = new AuthorizationPolicyBuilder()
                        .RequireRole(DefaultRoleEnumeration.Administrator.DisplayName)
                        .Build();
                    var administratorSystemPolicy = policyBuilder
                        .AddRequirements(new AdministratorSystemRequirement())
                        .Build();
                    var systemPolicy = new AuthorizationPolicyBuilder()
                        .RequireRole(DefaultRoleEnumeration.System.DisplayName)
                        .Build();

                    options.DefaultPolicy = userPolicy;
                    options.AddPolicy(AuthorizationExtension.JwtBearerUserPolicyName, userPolicy);
                    options.AddPolicy(AuthorizationExtension.JwtBearerUserSystemPolicyName, userSystemPolicy);
                    options.AddPolicy(AuthorizationExtension.JwtBearerAdministratorPolicyName, administratorPolicy);
                    options.AddPolicy(AuthorizationExtension.JwtBearerAdministratorSystemPolicyName, administratorSystemPolicy);
                    options.AddPolicy(AuthorizationExtension.JwtBearerSystemPolicyName, systemPolicy);

                    foreach (var policy in config.Policies)
                    {
                        options.AddPolicy(policy.Name, policy.Policy);
                    }
                });
        }
    }
}
using System;
using System.Security.Claims;
using IdentityModel;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;

namespace Riva.BuildingBlocks.WebApiTest.IntegrationTests
{
    public static class AuthUserOptions
    {
        public static Guid UserId = Guid.NewGuid();
        public const string Email = "email@email.com";

        public static ClaimsPrincipal CreateAdministratorClaimsPrincipal(string apResourceName, string scheme = AuthenticationExtension.JwtBearerAuthenticationScheme)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, Email),
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Email, Email),
                new Claim(JwtClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName),
                new Claim(JwtClaimTypes.Role, DefaultRoleEnumeration.Administrator.DisplayName),
                new Claim(ClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName),
                new Claim(ClaimTypes.Role, DefaultRoleEnumeration.Administrator.DisplayName),
                new Claim(JwtClaimTypes.Subject, UserId.ToString()),
                new Claim(JwtClaimTypes.Email, Email),
                new Claim(JwtClaimTypes.EmailVerified, true.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, apResourceName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public static ClaimsPrincipal CreateUserClaimsPrincipal(string apResourceName, string scheme = AuthenticationExtension.JwtBearerAuthenticationScheme)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, Email),
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                new Claim(ClaimTypes.Email, Email),
                new Claim(JwtClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName),
                new Claim(ClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName),
                new Claim(JwtClaimTypes.Subject, UserId.ToString()),
                new Claim(JwtClaimTypes.Email, Email),
                new Claim(JwtClaimTypes.EmailVerified, true.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, apResourceName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public static ClaimsPrincipal CreateSystemClaimsPrincipal(string apResourceName, string scheme = AuthenticationExtension.JwtBearerAuthenticationScheme)
        {
            var claims = new[] {
                new Claim("client_role", DefaultRoleEnumeration.System.DisplayName),
                new Claim(JwtClaimTypes.Scope, apResourceName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
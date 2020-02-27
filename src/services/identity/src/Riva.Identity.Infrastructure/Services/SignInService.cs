using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class SignInService : ISignInService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task SignInAsync(Guid accountId, string email, bool rememberLogin, IEnumerable<Claim> claims)
        {
            var props = rememberLogin
                ? new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AuthConstants.RememberMeLoginDuration)
                }
                : null;
            var issuer = new IdentityServerUser(accountId.ToString())
            {
                DisplayName = email,
                AdditionalClaims = claims.ToList()
            };
            return _httpContextAccessor.HttpContext.SignInAsync(issuer, props);
        }

        public async Task ExternalSignInAsync(Guid accountId, string email, string scheme, IEnumerable<Claim> claims)
        {
            var (authenticationProperties, additionalClaims, provider) = await ProcessExternalAuthAsync(scheme);
            var signInClaims = new List<Claim>(claims);
            signInClaims.AddRange(additionalClaims);
            var issuer = new IdentityServerUser(accountId.ToString())
            {
                DisplayName = email,
                IdentityProvider = provider,
                AdditionalClaims = signInClaims.ToList()
            };
            await _httpContextAccessor.HttpContext.SignInAsync(issuer, authenticationProperties);
        }

        private async Task<(AuthenticationProperties authenticationProperties, List<Claim> additionalClaims, string provider)> ProcessExternalAuthAsync(string scheme)
        {
            var authenticationProperties = new AuthenticationProperties();
            var additionalClaims = new List<Claim>();
            var authResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(scheme);

            var sid = authResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
                additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));

            const string tokenName = "id_token";
            var idToken = authResult.Properties.GetTokenValue(tokenName);
            if (idToken != null)
                authenticationProperties.StoreTokens(new[] { new AuthenticationToken { Name = tokenName, Value = idToken } });

            return (authenticationProperties, additionalClaims, authResult.Properties.Items["scheme"]);
        }
    }
}
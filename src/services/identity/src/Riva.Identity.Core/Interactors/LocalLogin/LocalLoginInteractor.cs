using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Clients.Repositories;

namespace Riva.Identity.Core.Interactors.LocalLogin
{
    public class LocalLoginInteractor : ILocalLoginInteractor
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IClientRepository _clientRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly ISchemeService _schemeService;
        private readonly IAccountClaimsCreatorService _accountClaimsCreatorService;
        private readonly ISignInService _signInService;

        public LocalLoginInteractor(IAuthorizationService authorizationService, IClientRepository clientRepository, 
            IAccountGetterService accountGetterService, IAccountVerificationService accountVerificationService, 
            ISchemeService schemeService, IAccountClaimsCreatorService accountClaimsCreatorService, ISignInService signInService)
        {
            _authorizationService = authorizationService;
            _clientRepository = clientRepository;
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _schemeService = schemeService;
            _accountClaimsCreatorService = accountClaimsCreatorService;
            _signInService = signInService;
        }

        public async Task<LocalLoginOutput> ExecuteAsync(string returnUrl)
        {
            var authRequest = await _authorizationService.GetAuthorizationRequestAsync(returnUrl);
            IEnumerable<LocalLoginExternalProviderOutput> externalProviders = new List<LocalLoginExternalProviderOutput>();

            if (!string.IsNullOrEmpty(authRequest?.IdP) && await _schemeService.GetSchemeAsync(authRequest.IdP) != null)
            {
                var local = authRequest.IdP == AuthConstants.LocalIdentityProvider;
                
                if (!local)
                    externalProviders = new List<LocalLoginExternalProviderOutput>
                    {
                        new LocalLoginExternalProviderOutput(string.Empty, authRequest.IdP)
                    };

                return new LocalLoginOutput(local, externalProviders);
            }

            var schemes = await _schemeService.GetAllSchemesAsync();
            var authenticationSchemes = schemes.Where(x => x.DisplayName != null);
            externalProviders = authenticationSchemes.Select(scheme =>
                new LocalLoginExternalProviderOutput(scheme.DisplayName ?? scheme.Name, scheme.Name));

            var allowLocalLogin = true;
            if (authRequest?.ClientId != null)
            {
                var client = await _clientRepository.GetByIdAsync(new Guid(authRequest.ClientId));
                if (client != null)
                {
                    allowLocalLogin = client.EnableLocalLogin;
                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                        externalProviders = externalProviders.Where(provider =>
                            client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme));
                }
            }

            return new LocalLoginOutput(allowLocalLogin, externalProviders);
        }

        public async Task<LocalLoginResultOutput> ExecuteAsync(string email, string password, bool rememberLogin, string returnUrl)
        {
            var authRequest = await _authorizationService.GetAuthorizationRequestAsync(returnUrl);
            var getAccountResult = await _accountGetterService.GetByEmailAsync(email);
            if (!getAccountResult.Success)
                return LocalLoginResultOutput.Fail(authRequest != null, getAccountResult.Errors);

            var accountCanBeAuthenticatedVerificationResult =
                _accountVerificationService.VerifyAccountCanBeAuthenticated(getAccountResult.Value, password);
            if (!accountCanBeAuthenticatedVerificationResult.Success)
                return LocalLoginResultOutput.Fail(authRequest != null,
                    accountCanBeAuthenticatedVerificationResult.Errors);

            var claims = await _accountClaimsCreatorService.CreateAccountClaimsAsync(getAccountResult.Value);
            await _signInService.SignInAsync(getAccountResult.Value.Id, getAccountResult.Value.Email, rememberLogin,
                claims);

            return LocalLoginResultOutput.Ok(authRequest != null, authRequest?.IsNativeClient);
        }
    }
}
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Core.Interactors.ExternalLogin
{
    public class ExternalLoginInteractor : IExternalLoginInteractor
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccountProviderService _accountProviderService;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountClaimsCreatorService _accountClaimsCreatorService;
        private readonly ISignInService _signInService;
        private readonly ISignOutService _signOutService;

        public ExternalLoginInteractor(IAuthorizationService authorizationService, IAccountProviderService accountProviderService,
            IIntegrationEventBus integrationEventBus, IAuthenticationService authenticationService, 
            IAccountClaimsCreatorService accountClaimsCreatorService, ISignInService signInService, ISignOutService signOutService)
        {
            _authorizationService = authorizationService;
            _accountProviderService = accountProviderService;
            _integrationEventBus = integrationEventBus;
            _authenticationService = authenticationService;
            _accountClaimsCreatorService = accountClaimsCreatorService;
            _signInService = signInService;
            _signOutService = signOutService;
        }

        public async Task<ExternalLoginResultOutput> ExecuteAsync(string scheme)
        {
            var authResult = await _authenticationService.AuthenticateAsync(scheme);
            if (!authResult.Succeeded)
                throw authResult.Failure;

            var emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);
            var correlationId = Guid.NewGuid();
            var account = await _accountProviderService.ProvideAccountForExternalLoginAsync(emailClaim.Value, correlationId);
            if (account.DomainEvents.Any(x => x.GetType() == typeof(AccountCreatedDomainEvent)))
            {
                var pictureClaim = authResult.Principal.FindFirst("picture");
                var picture = pictureClaim != null ? pictureClaim.Value : string.Empty;
                var accountCreatedIntegrationEvent = new AccountCreatedIntegrationEvent(correlationId, account.Id, account.Email, picture);
                await _integrationEventBus.PublishIntegrationEventAsync(accountCreatedIntegrationEvent);
            }

            var claims = await _accountClaimsCreatorService.CreateAccountClaimsAsync(account);
            var externalSignInTask = _signInService.ExternalSignInAsync(account.Id, account.Email, scheme, claims);
            var signOutTask = _signOutService.SignOutAsync(scheme);

            var returnUrl =
                authResult.Items is null || !authResult.Items.ContainsKey("returnUrl") ||
                string.IsNullOrWhiteSpace(authResult.Items["returnUrl"])
                    ? "~/"
                    : authResult.Items["returnUrl"];

            var authRequest = await _authorizationService.GetAuthorizationRequestAsync(returnUrl);
            await Task.WhenAll(externalSignInTask, signOutTask);
            return new ExternalLoginResultOutput(returnUrl, authRequest?.IsNativeClient);
        }
    }
}
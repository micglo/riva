using System;

namespace Riva.Identity.Web.Models.ViewModels
{
    public class LoggedOutViewModel
    {
        public string LogoutId { get; }
        public string PostLogoutRedirectUri { get; }
        public string SignOutIframeUrl { get; }
        public Guid? ClientId { get; }
        public string ExternalAuthenticationScheme { get; }
        public bool TriggerExternalSignOut { get; }

        public LoggedOutViewModel(string logoutId, string postLogoutRedirectUri, string signOutIframeUrl,
            Guid? clientId, string externalAuthenticationScheme)
        {
            LogoutId = logoutId;
            PostLogoutRedirectUri = postLogoutRedirectUri;
            SignOutIframeUrl = signOutIframeUrl;
            ClientId = clientId;
            ExternalAuthenticationScheme = externalAuthenticationScheme;
            TriggerExternalSignOut = ExternalAuthenticationScheme != null;
        }
    }
}
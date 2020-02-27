using System;

namespace Riva.Identity.Core.Interactors.Logout
{
    public class LoggedOutOutput
    {
        public string LogoutId { get; }
        public string PostLogoutRedirectUri { get; }
        public string SignOutIframeUrl { get; }
        public Guid? ClientId { get; }
        public string ExternalAuthenticationScheme { get; }

        public LoggedOutOutput(string logoutId, string postLogoutRedirectUri, string signOutIframeUrl,
            Guid? clientId, string externalAuthenticationScheme)
        {
            LogoutId = logoutId;
            PostLogoutRedirectUri = postLogoutRedirectUri;
            SignOutIframeUrl = signOutIframeUrl;
            ClientId = clientId;
            ExternalAuthenticationScheme = externalAuthenticationScheme;
        }
    }
}
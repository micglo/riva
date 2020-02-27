using System;

namespace Riva.Identity.Core.Models
{
    public class AuthorizationRequest
    {
        public string IdP { get; }
        public string ClientId { get; }
        public bool IsNativeClient { get; }

        public AuthorizationRequest(string idP, string clientId, string redirectUri)
        {
            IdP = idP;
            ClientId = clientId;
            IsNativeClient = !redirectUri.StartsWith("https", StringComparison.Ordinal) &&
                             !redirectUri.StartsWith("http", StringComparison.Ordinal);
        }
    }
}
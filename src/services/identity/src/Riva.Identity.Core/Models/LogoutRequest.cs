using System;

namespace Riva.Identity.Core.Models
{
    public class LogoutRequest
    {
        public bool ShowSignOutPrompt { get; }
        public string PostLogoutRedirectUri { get; }
        public string SignOutIFrameUrl { get; }
        public Guid? SubjectId { get; }
        public Guid? ClientId { get; }

        public LogoutRequest(bool showSignOutPrompt, string postLogoutRedirectUri, string signOutIFrameUrl, Guid? subjectId, Guid? clientId)
        {
            ShowSignOutPrompt = showSignOutPrompt;
            PostLogoutRedirectUri = postLogoutRedirectUri;
            SignOutIFrameUrl = signOutIFrameUrl;
            SubjectId = subjectId;
            ClientId = clientId;
        }
    }
}
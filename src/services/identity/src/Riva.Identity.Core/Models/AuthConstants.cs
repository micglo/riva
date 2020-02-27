using System;

namespace Riva.Identity.Core.Models
{
    public static class AuthConstants
    {
        public static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static readonly string LocalIdentityProvider = "local";
    }
}
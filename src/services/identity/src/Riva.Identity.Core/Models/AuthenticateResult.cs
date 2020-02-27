using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Riva.Identity.Core.Models
{
    public class AuthenticateResult
    {
        public bool Succeeded { get; }
        public Exception Failure { get; }
        public ClaimsPrincipal Principal { get; }
        public IDictionary<string, string> Items { get; }

        public AuthenticateResult(bool succeeded, Exception failure, ClaimsPrincipal principal, IDictionary<string, string> items)
        {
            Succeeded = succeeded;
            Failure = failure;
            Principal = principal;
            Items = items;
        }
    }
}
using System;
using Riva.Identity.Web.Api.Models.Enums;

namespace Riva.Identity.Web.Api.Models.Responses.Accounts
{
    public class AccountToken
    {
        public DateTimeOffset Issued { get; }
        public DateTimeOffset Expires { get; }
        public AccountTokenType Type { get; }
        public string Value { get; }

        public AccountToken(DateTimeOffset issued, DateTimeOffset expires, AccountTokenType type, string value)
        {
            Issued = issued;
            Expires = expires;
            Type = type;
            Value = value;
        }
    }
}
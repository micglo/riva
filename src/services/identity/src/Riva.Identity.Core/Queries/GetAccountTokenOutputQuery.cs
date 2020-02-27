using System;
using Riva.Identity.Domain.Accounts.Enumerations;

namespace Riva.Identity.Core.Queries
{
    public class GetAccountTokenOutputQuery
    {
        public DateTimeOffset Issued { get; }
        public DateTimeOffset Expires { get; }
        public TokenTypeEnumeration Type { get; }
        public string Value { get; }

        public GetAccountTokenOutputQuery(DateTimeOffset issued, DateTimeOffset expires, TokenTypeEnumeration type, string value)
        {
            Issued = issued;
            Expires = expires;
            Type = type;
            Value = value;
        }
    }
}
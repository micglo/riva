using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Identity.Web.Api.Models.Responses.Accounts
{
    public class GetAccountResponse : ResponseBase
    {
        public string Email { get; }
        public bool Confirmed { get; }
        public DateTimeOffset Created { get; }
        public bool PasswordAssigned { get; }
        public DateTimeOffset? LastLogin { get; }
        public IReadOnlyCollection<Guid> Roles { get; }
        public IReadOnlyCollection<AccountToken> Tokens { get; }

        public GetAccountResponse(Guid id, string email, bool confirmed, DateTimeOffset created,
            bool passwordAssigned, DateTimeOffset? lastLogin, IEnumerable<Guid> roles, 
            IEnumerable<AccountToken> tokens) : base(id)
        {
            Email = email;
            Confirmed = confirmed;
            Created = created;
            PasswordAssigned = passwordAssigned;
            LastLogin = lastLogin;
            Roles = roles.ToList().AsReadOnly();
            Tokens = tokens.ToList().AsReadOnly();
        }
    }
}
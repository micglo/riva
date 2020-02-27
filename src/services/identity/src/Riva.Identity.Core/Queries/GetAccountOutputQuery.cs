using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Identity.Core.Queries
{
    public class GetAccountOutputQuery : OutputQueryBase
    {
        public string Email { get; }
        public bool Confirmed { get; }
        public DateTimeOffset Created { get; }
        public bool PasswordAssigned { get; }
        public DateTimeOffset? LastLogin { get; }
        public IReadOnlyCollection<Guid> Roles { get; }
        public IReadOnlyCollection<GetAccountTokenOutputQuery> Tokens { get; }

        public GetAccountOutputQuery(Guid id, string email, bool confirmed, DateTimeOffset created,
            bool passwordAssigned, DateTimeOffset? lastLogin, IEnumerable<Guid> roles, 
            IEnumerable<GetAccountTokenOutputQuery> tokens) : base(id)
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
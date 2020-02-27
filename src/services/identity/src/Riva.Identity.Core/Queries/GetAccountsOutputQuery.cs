using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Identity.Core.Queries
{
    public class GetAccountsOutputQuery : OutputQueryBase
    {
        public string Email { get; }
        public bool Confirmed { get; }
        public DateTimeOffset Created { get; }
        public bool PasswordAssigned { get; }
        public DateTimeOffset? LastLogin { get; }

        public GetAccountsOutputQuery(Guid id, string email, bool confirmed, DateTimeOffset created,
            bool passwordAssigned, DateTimeOffset? lastLogin) : base(id)
        {
            Email = email;
            Confirmed = confirmed;
            Created = created;
            PasswordAssigned = passwordAssigned;
            LastLogin = lastLogin;
        }
    }
}
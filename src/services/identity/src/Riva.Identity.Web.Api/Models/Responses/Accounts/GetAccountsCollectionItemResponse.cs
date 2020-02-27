using System;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Identity.Web.Api.Models.Responses.Accounts
{
    public class GetAccountsCollectionItemResponse : ResponseBase
    {
        public string Email { get; }
        public bool Confirmed { get; }
        public DateTimeOffset Created { get; }
        public bool PasswordAssigned { get; }
        public DateTimeOffset? LastLogin { get; }

        public GetAccountsCollectionItemResponse(Guid id, string email, bool confirmed, DateTimeOffset created,
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
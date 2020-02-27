using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities
{
    public class AccountEntity : EntityBase
    {
        public string Email { get; set; }
        public bool Confirmed { get; set; }
        public string PasswordHash { get; set; }
        public Guid SecurityStamp { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public ICollection<AccountRoleEntity> Roles { get; set; }
        public ICollection<TokenEntity> Tokens { get; set; }

        public AccountEntity()
        {
            Roles = new List<AccountRoleEntity>();
            Tokens = new List<TokenEntity>();
        }
    }
}
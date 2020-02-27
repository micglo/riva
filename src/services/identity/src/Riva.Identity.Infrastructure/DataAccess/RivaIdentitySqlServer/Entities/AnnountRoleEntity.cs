using System;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities
{
    public class AccountRoleEntity
    {
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }
        public AccountEntity Account { get; set; }
        public RoleEntity Role { get; set; }
    }
}
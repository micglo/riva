using System.Collections.Generic;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities
{
    public class RoleEntity : VersionedEntityBase
    {
        public string Name { get; set; }
        public ICollection<AccountRoleEntity> Accounts { get; set; }

        public RoleEntity()
        {
            Accounts = new List<AccountRoleEntity>();
        }
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Configs
{
    public static class AccountRoleEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<AccountRoleEntity> entity)
        {
            entity.HasKey(x => new { x.AccountId, x.RoleId });

            entity
                .HasOne(x => x.Account)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.AccountId);

            entity
                .HasOne(x => x.Role)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
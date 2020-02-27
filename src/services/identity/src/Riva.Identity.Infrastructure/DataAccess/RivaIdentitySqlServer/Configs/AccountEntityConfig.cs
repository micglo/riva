using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Configs
{
    public static class AccountEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<AccountEntity> entity)
        {
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.Property(x => x.Confirmed).IsRequired();
            entity.Property(x => x.SecurityStamp).IsRequired();
            entity.Property(x => x.Created).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
        }
    }
}
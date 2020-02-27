using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Enums;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Configs
{
    public static class TokenEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<TokenEntity> entity)
        {
            entity.Property(x => x.Issued).IsRequired();
            entity.Property(x => x.Expires).IsRequired();
            entity.Property(x => x.Type).IsRequired().HasConversion(new EnumToStringConverter<TokenType>());
            entity.Property(x => x.Value).IsRequired();
            entity.HasOne(x => x.Account).WithMany(x => x.Tokens).HasForeignKey(x => x.AccountId);
        }
    }
}
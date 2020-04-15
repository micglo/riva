using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Riva.Users.Core.Enums;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Configs
{
    public static class UserEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<UserEntity> entity)
        {
            entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
            entity.Property(x => x.Picture).HasMaxLength(256);
            entity.Property(x => x.ServiceActive).IsRequired();
            entity.Property(x => x.AnnouncementPreferenceLimit).IsRequired();
            entity.Property(x => x.AnnouncementSendingFrequency).IsRequired().HasConversion(new EnumToStringConverter<AnnouncementSendingFrequency>());
            entity.HasIndex(x => x.Email).IsUnique();
        }
    }
}
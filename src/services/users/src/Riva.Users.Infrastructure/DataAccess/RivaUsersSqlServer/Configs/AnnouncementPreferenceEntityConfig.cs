using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Riva.Users.Core.Enums;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Configs
{
    public static class AnnouncementPreferenceEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<AnnouncementPreferenceEntity> entity)
        {
            entity.Property(x => x.CityId).IsRequired();
            entity.Property(x => x.AnnouncementPreferenceType).IsRequired().HasConversion(new EnumToStringConverter<AnnouncementPreferenceType>());
            entity.Property(x => x.RoomType).HasConversion(new EnumToStringConverter<RoomType>());
            entity.Property(x => x.PriceMin).HasColumnType("decimal(38, 2)");
            entity.Property(x => x.PriceMax).HasColumnType("decimal(38, 2)");

            entity
                .HasOne(x => x.User)
                .WithMany(x => x.AnnouncementPreferences)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
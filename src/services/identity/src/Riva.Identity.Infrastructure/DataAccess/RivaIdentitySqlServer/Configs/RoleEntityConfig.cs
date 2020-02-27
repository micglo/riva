using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Configs
{
    public static class RoleEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<RoleEntity> entity)
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
            entity.Property(x => x.RowVersion).IsRequired().IsRowVersion();
            entity.HasIndex(x => x.Name).IsUnique();

            entity.HasData(new RoleEntity{Id = Guid.NewGuid(), Name = DefaultRoleEnumeration.User.DisplayName});
            entity.HasData(new RoleEntity{Id = Guid.NewGuid(), Name = DefaultRoleEnumeration.Administrator.DisplayName});
            entity.HasData(new RoleEntity{Id = Guid.NewGuid(), Name = DefaultRoleEnumeration.System.DisplayName});
        }
    }
}
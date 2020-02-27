using Microsoft.EntityFrameworkCore;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Configs;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Configs;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts
{
    public class RivaIdentityDbContext : DbContext
    {
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<AccountRoleEntity> AccountRoles { get; set; }
        public DbSet<TokenEntity> Tokens { get; set; }
        public DbSet<DomainEventEntity> DomainEvents { get; set; }

        public RivaIdentityDbContext(DbContextOptions<RivaIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>().Configure();
            modelBuilder.Entity<AccountEntity>().Configure();
            modelBuilder.Entity<AccountRoleEntity>().Configure();
            modelBuilder.Entity<TokenEntity>().Configure();
            modelBuilder.Entity<DomainEventEntity>().Configure();
        }
    }
}
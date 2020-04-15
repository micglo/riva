using Microsoft.EntityFrameworkCore;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Configs;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Configs;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts
{
    public class RivaUsersDbContext : DbContext
    {
        public DbSet<DomainEventEntity> DomainEvents { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AnnouncementPreferenceEntity> AnnouncementPreferences { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CityDistrictEntity> CityDistricts { get; set; }
        public DbSet<AnnouncementPreferenceCityDistrictEntity> AnnouncementPreferenceCityDistricts { get; set; }

        public RivaUsersDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DomainEventEntity>().Configure();
            modelBuilder.Entity<UserEntity>().Configure();
            modelBuilder.Entity<AnnouncementPreferenceEntity>().Configure();
            modelBuilder.Entity<CityEntity>().Configure();
            modelBuilder.Entity<CityDistrictEntity>().Configure();
            modelBuilder.Entity<AnnouncementPreferenceCityDistrictEntity>().Configure();
        }
    }
}
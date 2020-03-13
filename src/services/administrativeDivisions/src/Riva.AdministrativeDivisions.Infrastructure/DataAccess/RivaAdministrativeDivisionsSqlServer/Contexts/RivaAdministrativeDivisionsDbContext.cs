using Microsoft.EntityFrameworkCore;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Configs;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts
{
    public class RivaAdministrativeDivisionsDbContext : DbContext
    {
        public DbSet<StateEntity> States { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CityDistrictEntity> CityDistricts { get; set; }
        public DbSet<CityDistrictNameVariantEntity> CityDistrictNameVariants { get; set; }

        public RivaAdministrativeDivisionsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StateEntity>().Configure();
            modelBuilder.Entity<CityEntity>().Configure();
            modelBuilder.Entity<CityDistrictEntity>().Configure();
            modelBuilder.Entity<CityDistrictNameVariantEntity>().Configure();
        }
    }
}
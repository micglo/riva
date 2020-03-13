using System.Linq;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Extensions
{
    public static class DatabaseCleaner
    {
        public static void ClearDatabase(this RivaAdministrativeDivisionsDbContext dbContext)
        {
            RemoveCityDistrictNameVariants(dbContext);
            RemoveCityDistricts(dbContext);
            RemoveCities(dbContext);
            RemoveStates(dbContext);
            dbContext.SaveChanges();
        }

        private static void RemoveCityDistrictNameVariants(RivaAdministrativeDivisionsDbContext dbContext)
        {
            var cityDistrictPolishNameVariants = dbContext.CityDistrictNameVariants.ToList();
            dbContext.CityDistrictNameVariants.RemoveRange(cityDistrictPolishNameVariants);
        }

        private static void RemoveCityDistricts(RivaAdministrativeDivisionsDbContext dbContext)
        {
            var cityDistricts = dbContext.CityDistricts.ToList();
            dbContext.CityDistricts.RemoveRange(cityDistricts);
        }

        private static void RemoveCities(RivaAdministrativeDivisionsDbContext dbContext)
        {
            var cities = dbContext.Cities.ToList();
            dbContext.Cities.RemoveRange(cities);
        }

        private static void RemoveStates(RivaAdministrativeDivisionsDbContext dbContext)
        {
            var states = dbContext.States.ToList();
            dbContext.States.RemoveRange(states);
        }
    }
}
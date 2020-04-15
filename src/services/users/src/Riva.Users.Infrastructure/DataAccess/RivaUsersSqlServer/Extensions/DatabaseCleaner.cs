using System.Linq;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Extensions
{
    public static class DatabaseCleaner
    {
        public static void ClearDatabase(this RivaUsersDbContext dbContext)
        {
            RemoveCities(dbContext);
            RemoveUsers(dbContext);
            RemoveDomainEvents(dbContext);
            dbContext.SaveChanges();
        }

        private static void RemoveCities(RivaUsersDbContext dbContext)
        {
            var cities = dbContext.Cities.ToList();
            dbContext.Cities.RemoveRange(cities);
        }

        private static void RemoveUsers(RivaUsersDbContext dbContext)
        {
            var users = dbContext.Users.ToList();
            dbContext.Users.RemoveRange(users);
        }

        private static void RemoveDomainEvents(RivaUsersDbContext dbContext)
        {
            var domainEvents = dbContext.DomainEvents.ToList();
            dbContext.DomainEvents.RemoveRange(domainEvents);
        }
    }
}
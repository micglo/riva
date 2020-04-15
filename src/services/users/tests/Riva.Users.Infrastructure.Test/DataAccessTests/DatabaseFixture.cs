using System;
using Microsoft.EntityFrameworkCore;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Extensions;

namespace Riva.Users.Infrastructure.Test.DataAccessTests
{
    public class DatabaseFixture : IDisposable
    {
        public RivaUsersDbContext Context { get; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<RivaUsersDbContext>()
                .UseInMemoryDatabase("RivaUsersDatabaseIntegrationTestsDb").Options;
            Context = new RivaUsersDbContext(options);
            Context.ClearDatabase();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                Context.Dispose();
        }
    }
}
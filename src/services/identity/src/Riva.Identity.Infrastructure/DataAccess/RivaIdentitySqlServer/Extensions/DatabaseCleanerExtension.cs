using System.Linq;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions
{
    public static class DatabaseCleanerExtension
    {
        public static void ClearDatabase(this RivaIdentityDbContext context)
        {
            RemoveAccounts(context);
            RemoveRoles(context);
            context.SaveChanges();
        }

        private static void RemoveAccounts(RivaIdentityDbContext context)
        {
            var accounts = context.Accounts.ToList();
            context.Accounts.RemoveRange(accounts);
        }

        private static void RemoveRoles(RivaIdentityDbContext context)
        {
            var roles = context.Roles.ToList();
            context.Roles.RemoveRange(roles);
        }
    }
}
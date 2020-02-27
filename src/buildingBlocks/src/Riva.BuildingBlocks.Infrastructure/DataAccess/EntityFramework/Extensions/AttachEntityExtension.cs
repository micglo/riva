using System.Linq;
using Microsoft.EntityFrameworkCore;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Extensions
{
    public static class AttachEntityExtension
    {
        public static void AttachEntity<TEntity>(this DbContext context, TEntity entity) where TEntity : EntityBase
        {
            var dbSet = context.Set<TEntity>();

            var localEntity = dbSet.Local.FirstOrDefault(x => x.Id == entity.Id);

            if (localEntity != null)
                context.Entry(localEntity).State = EntityState.Detached;

            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
        }
    }
}
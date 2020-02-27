using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Repositories;
using Riva.BuildingBlocks.Infrastructure.Stores;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class DomainEventsStoreExtension
    {
        public static IServiceCollection AddDomainEventsStore<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            return services
                .AddScoped<IDomainEventRepository>(x => new DomainEventRepository(x.GetRequiredService<TDbContext>()))
                .AddScoped<IDomainEventStore, DomainEventStore>();
        }
    }
}
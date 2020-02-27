using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Configs
{
    public static class DomainEventEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<DomainEventEntity> entity)
        {
            entity.Property(x => x.AggregateId).IsRequired();
            entity.Property(x => x.CorrelationId).IsRequired();
            entity.Property(x => x.CreationDate).IsRequired();
            entity.Property(x => x.FullyQualifiedEventTypeName).IsRequired();
            entity.Property(x => x.EventData).IsRequired();
        }
    }
}
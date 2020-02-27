using System;

namespace Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
    }
}
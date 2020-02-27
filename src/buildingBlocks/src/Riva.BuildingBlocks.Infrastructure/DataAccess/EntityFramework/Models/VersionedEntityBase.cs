namespace Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models
{
    public abstract class VersionedEntityBase : EntityBase
    {
        public byte[] RowVersion { get; set; }
    }
}
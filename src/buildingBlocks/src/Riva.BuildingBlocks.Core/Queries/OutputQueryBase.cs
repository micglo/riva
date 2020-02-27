using System;

namespace Riva.BuildingBlocks.Core.Queries
{
    public abstract class OutputQueryBase : IOutputQuery
    {
        public Guid Id { get; }

        protected OutputQueryBase(Guid id)
        {
            Id = id;
        }
    }
}
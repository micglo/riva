using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class GetStateInputQuery : IInputQuery
    {
        public Guid StateId { get; }

        public GetStateInputQuery(Guid stateId)
        {
            StateId = stateId;
        }
    }
}
using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Responses
{
    public class StateResponse : VersionedResponseBase
    {
        public string Name { get; }
        public string PolishName { get; }

        public StateResponse(Guid id, IEnumerable<byte> rowVersion, string name, string polishName) : base(id, rowVersion)
        {
            Name = name;
            PolishName = polishName;
        }
    }
}
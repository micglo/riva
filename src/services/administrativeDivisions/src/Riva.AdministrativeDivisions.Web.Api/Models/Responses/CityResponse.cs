using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Responses
{
    public class CityResponse : VersionedResponseBase
    {
        public string Name { get; }
        public string PolishName { get; }
        public Guid StateId { get; }

        public CityResponse(Guid id, IEnumerable<byte> rowVersion, string name, string polishName, Guid stateId) : base(id, rowVersion)
        {
            Name = name;
            PolishName = polishName;
            StateId = stateId;
        }
    }
}
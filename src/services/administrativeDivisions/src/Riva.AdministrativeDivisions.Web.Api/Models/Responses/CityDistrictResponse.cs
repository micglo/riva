using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Responses
{
    public class CityDistrictResponse : VersionedResponseBase
    {
        public string Name { get; }
        public string PolishName { get; }
        public Guid CityId { get; }
        public Guid? ParentId { get; }
        public IReadOnlyCollection<string> NameVariants { get; }

        public CityDistrictResponse(Guid id, IEnumerable<byte> rowVersion, string name,
            string polishName, Guid cityId, Guid? parentId, IEnumerable<string> nameVariants) : base(id, rowVersion)
        {
            Name = name;
            PolishName = polishName;
            ParentId = parentId;
            CityId = cityId;
            NameVariants = nameVariants.ToList().AsReadOnly();
        }
    }
}
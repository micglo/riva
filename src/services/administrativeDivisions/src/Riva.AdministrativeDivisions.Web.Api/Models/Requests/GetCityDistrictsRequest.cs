using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class GetCityDistrictsRequest : CollectionRequestBase
    {
        public GetCityDistrictsRequest()
        {
            CityIds = new List<Guid>();
        }

        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string PolishName { get; set; }

        public Guid? CityId { get; set; }
        public Guid? ParentId { get; set; }

        [AllowedValues("name:asc", "name::desc", "polishName:asc", "polishName::desc")]
        public string Sort { get; set; }

        [GuidCollection(true, false, false)]
        public IEnumerable<Guid> CityIds { get; set; }
    }
}
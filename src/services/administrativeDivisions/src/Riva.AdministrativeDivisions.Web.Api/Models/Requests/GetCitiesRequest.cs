using System;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class GetCitiesRequest : CollectionRequestBase
    {
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string PolishName { get; set; }

        public Guid? StateId { get; set; }

        [AllowedValues("name:asc", "name::desc", "polishName:asc", "polishName::desc")]
        public string Sort { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class UpdateCityDistrictRequest : VersionedUpdateRequestBase
    {
        public UpdateCityDistrictRequest()
        {
            NameVariants = new Collection<string>();
        }

        [Required] 
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string PolishName { get; set; }

        [Required]
        public Guid CityId { get; set; }

        public Guid? ParentId { get; set; }

        [StringCollection(true, false, false)]
        public IEnumerable<string> NameVariants { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class UpdateCityRequest : VersionedUpdateRequestBase
    {
        [Required]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string PolishName { get; set; }

        [Required]
        public Guid StateId { get; set; }
    }
}
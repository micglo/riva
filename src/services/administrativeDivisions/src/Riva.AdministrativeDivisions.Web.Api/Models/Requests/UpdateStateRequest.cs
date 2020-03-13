using System;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class UpdateStateRequest : VersionedUpdateRequestBase
    {
        [Required] 
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string PolishName { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class CreateCityRequest
    {
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
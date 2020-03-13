using System.ComponentModel.DataAnnotations;

namespace Riva.AdministrativeDivisions.Web.Api.Models.Requests
{
    public class CreateStateRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string PolishName { get; set; }
    }
}
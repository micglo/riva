using System.ComponentModel.DataAnnotations;

namespace Riva.Identity.Web.Api.Models.Requests.Roles
{
    public class CreateRoleRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
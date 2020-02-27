using System.ComponentModel.DataAnnotations;

namespace Riva.Identity.Web.Api.Models.Requests.Accounts
{
    public class AssignPasswordRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
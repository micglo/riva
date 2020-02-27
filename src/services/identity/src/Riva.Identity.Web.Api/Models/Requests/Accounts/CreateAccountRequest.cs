using System.ComponentModel.DataAnnotations;

namespace Riva.Identity.Web.Api.Models.Requests.Accounts
{
    public class CreateAccountRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 6)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
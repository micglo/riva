using System.ComponentModel.DataAnnotations;

namespace Riva.Identity.Web.Api.Models.Requests.Accounts
{
    public class ChangePasswordRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Riva.Identity.Web.Api.Models.Requests.Accounts
{
    public class RequestPasswordResetTokenRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
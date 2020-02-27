using System.ComponentModel.DataAnnotations;

namespace Riva.Identity.Web.Models.Requests
{
    public class LocalLoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using Riva.Users.Core.Enums;

namespace Riva.Users.Web.Api.Models.Requests
{
    public class CreateUserRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool ServiceActive { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AnnouncementPreferenceLimit { get; set; }

        [Required]
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; set; }
    }
}
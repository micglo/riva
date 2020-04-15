using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Riva.Users.Core.Enums;

namespace Riva.Users.Web.Api.Models.Requests
{
    public class UpdateUserRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool ServiceActive { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AnnouncementPreferenceLimit { get; set; }

        [Required]
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; set; }

        public IFormFile Picture { get; set; }
    }
}
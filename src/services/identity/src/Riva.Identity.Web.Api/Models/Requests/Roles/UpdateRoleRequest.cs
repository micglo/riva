using System;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;

namespace Riva.Identity.Web.Api.Models.Requests.Roles
{
    public class UpdateRoleRequest : VersionedUpdateRequestBase
    {
        [Required]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
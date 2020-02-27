using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.Identity.Web.Api.Models.Requests.Accounts
{
    public class UpdateAccountRolesRequest
    {
        public UpdateAccountRolesRequest()
        {
            Roles = new List<Guid>();
        }

        [Required]
        [GuidCollection(false, false, false)]
        public IEnumerable<Guid> Roles { get; set; }
    }
}
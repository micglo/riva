using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.Users.Web.Api.Models.Requests
{
    public class GetUsersRequest : CollectionRequestBase
    {
        [StringLength(256)]
        public string Email { get; set; }

        public bool? ServiceActive { get; set; }

        [AllowedValues("email:asc", "email:desc")]
        public string Sort { get; set; }
    }
}
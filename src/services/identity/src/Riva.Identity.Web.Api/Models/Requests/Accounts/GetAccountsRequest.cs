using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.Models.Requests;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.Identity.Web.Api.Models.Requests.Accounts
{
    public class GetAccountsRequest : CollectionRequestBase
    {
        [StringLength(256)]
        public string Email { get; set; }

        public bool? Confirmed { get; set; }

        [AllowedValues("email:asc", "email:desc", "created:asc", "created:desc")]
        public string Sort { get; set; }
    }
}
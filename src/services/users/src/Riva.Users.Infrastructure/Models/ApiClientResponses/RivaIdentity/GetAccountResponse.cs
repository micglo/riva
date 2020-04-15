using System;
using Newtonsoft.Json;
using Riva.Users.Core.Models;

namespace Riva.Users.Infrastructure.Models.ApiClientResponses.RivaIdentity
{
    public class GetAccountResponse : IAccount
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
using System;
using Newtonsoft.Json;
using Riva.Announcements.Core.Models;

namespace Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions
{
    public class GetCityResponse : ICity
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("rowVersion")]
        public byte[] RowVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("polishName")]
        public string PolishName { get; set; }

        [JsonProperty("stateId")]
        public Guid StateId { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions
{
    public class GetCityDistrictsResponse
    {
        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("results")]
        public IEnumerable<CityDistrict> Results { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Riva.Announcements.Core.Models;

namespace Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions
{
    public class CityDistrict : ICityDistrict
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("rowVersion")]
        public byte[] RowVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("polishName")]
        public string PolishName { get; set; }

        [JsonProperty("cityId")]
        public Guid CityId { get; set; }

        [JsonProperty("parentId")]
        public Guid? ParentId { get; set; }

        [JsonProperty("nameVariants")]
        public IReadOnlyCollection<string> NameVariants { get; set; }
    }
}
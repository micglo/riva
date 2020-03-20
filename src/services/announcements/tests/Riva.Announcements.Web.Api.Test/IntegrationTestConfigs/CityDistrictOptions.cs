using System;
using System.Collections.Generic;
using Riva.Announcements.Core.Models;
using Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
{
    public class CityDistrictOptions
    {
        public static IReadOnlyCollection<ICityDistrict> CityDistricts => new List<ICityDistrict>
        {
            new CityDistrict
            {
                Id = new Guid("e33b4304-9645-460f-a8d3-a218d08629e8"),
                Name = "Name",
                PolishName = "PolishName",
                RowVersion = Array.Empty<byte>(),
                CityId = CityOptions.City.Id,
                NameVariants = new List<string>()
            }
        };
    }
}
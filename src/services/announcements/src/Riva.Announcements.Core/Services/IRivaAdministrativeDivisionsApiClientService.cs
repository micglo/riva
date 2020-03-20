using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Announcements.Core.Models;

namespace Riva.Announcements.Core.Services
{
    public interface IRivaAdministrativeDivisionsApiClientService
    {
        Task<ICity> GetCityAsync(Guid cityId);
        Task<IEnumerable<ICityDistrict>> GetCityDistrictsAsync(Guid cityId);
    }
}
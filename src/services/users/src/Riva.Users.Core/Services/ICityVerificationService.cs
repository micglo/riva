using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Users.Core.Services
{
    public interface ICityVerificationService
    {
        Task<VerificationResult> VerifyCityAndCityDistrictsAsync(Guid cityId, IEnumerable<Guid> cityDistrictIds);
    }
}
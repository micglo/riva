using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Core.Services
{
    public interface ICityDistrictVerificationService
    {
        Task<VerificationResult> VerifyCityDistrictsExistAsync(Guid cityId, IEnumerable<Guid> cityDistrictIds);
    }
}
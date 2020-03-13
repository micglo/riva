using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Services
{
    public interface ICityDistrictVerificationService
    {
        Task<VerificationResult> VerifyParentExistsAsync(Guid parentId);
        Task<VerificationResult> VerifyNameIsNotTakenAsync(string name, Guid cityId);
        Task<VerificationResult> VerifyPolishNameIsNotTakenAsync(string name, Guid cityId);
    }
}
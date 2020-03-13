using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Services
{
    public interface ICityVerificationService
    {
        Task<VerificationResult> VerifyNameIsNotTakenAsync(string name, Guid stateId);
        Task<VerificationResult> VerifyPolishNameIsNotTakenAsync(string polishName, Guid stateId);
    }
}
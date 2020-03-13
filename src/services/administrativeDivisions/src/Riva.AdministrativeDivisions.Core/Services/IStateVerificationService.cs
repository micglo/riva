using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Services
{
    public interface IStateVerificationService
    {
        Task<VerificationResult> VerifyNameIsNotTakenAsync(string name);
        Task<VerificationResult> VerifyPolishNameIsNotTakenAsync(string polishName);
    }
}
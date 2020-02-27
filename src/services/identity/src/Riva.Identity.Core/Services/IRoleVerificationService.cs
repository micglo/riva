using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface IRoleVerificationService
    {
        Task<VerificationResult> VerifyNameIsNotTakenAsync(string name);
    }
}
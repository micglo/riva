using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.Core.Models
{
    public class VerificationResult
    {
        public bool Success { get; }
        public IReadOnlyCollection<IError> Errors { get; }

        private VerificationResult(bool success, IEnumerable<IError> errors)
        {
            Success = success;
            Errors = errors.ToList().AsReadOnly();
        }
        
        public static VerificationResult Ok()
        {
            return new VerificationResult(true, new List<IError>());
        }

        public static VerificationResult Fail(IEnumerable<IError> errors)
        {
            return new VerificationResult(false, errors);
        }
    }
}
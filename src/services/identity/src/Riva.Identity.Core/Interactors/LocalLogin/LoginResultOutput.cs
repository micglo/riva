using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Identity.Core.Interactors.LocalLogin
{
    public class LocalLoginResultOutput
    {
        public bool Success { get; }
        public IReadOnlyCollection<IError> Errors { get; }
        public bool IsInTheContextOfAuthorizationRequest { get; }
        public bool? IsNativeClient { get; }

        private LocalLoginResultOutput(bool success, IEnumerable<IError> errors, 
            bool isInTheContextOfAuthorizationRequest, bool? isNativeClient = null)
        {
            Success = success;
            Errors = errors.ToList().AsReadOnly();
            IsInTheContextOfAuthorizationRequest = isInTheContextOfAuthorizationRequest;
            IsNativeClient = isNativeClient;
        }

        public static LocalLoginResultOutput Ok(bool isInTheContextOfAuthorizationRequest, bool? isNativeClient)
        {
            return new LocalLoginResultOutput(true, new List<IError>(), isInTheContextOfAuthorizationRequest,
                isNativeClient);
        }

        public static LocalLoginResultOutput Fail(bool isInTheContextOfAuthorizationRequest, IEnumerable<IError> errors)
        {
            return new LocalLoginResultOutput(false, errors, isInTheContextOfAuthorizationRequest);
        }
    }
}
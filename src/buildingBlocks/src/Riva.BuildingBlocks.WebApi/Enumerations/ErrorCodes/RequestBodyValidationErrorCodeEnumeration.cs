using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes
{
    public class RequestBodyValidationErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static RequestBodyValidationErrorCodeEnumeration InvalidValue => new RequestBodyValidationErrorCodeEnumeration(1, nameof(InvalidValue));

        private RequestBodyValidationErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
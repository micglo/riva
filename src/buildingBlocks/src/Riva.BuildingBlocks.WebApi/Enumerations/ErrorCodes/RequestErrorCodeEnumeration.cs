using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes
{
    public class RequestErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static RequestErrorCodeEnumeration BadRequest => new RequestErrorCodeEnumeration(1, nameof(BadRequest));
        public static RequestErrorCodeEnumeration AuthorizationRequired => new RequestErrorCodeEnumeration(2, nameof(AuthorizationRequired));
        public static RequestErrorCodeEnumeration AccessDenied => new RequestErrorCodeEnumeration(3, nameof(AccessDenied));
        public static RequestErrorCodeEnumeration ResourceNotFound => new RequestErrorCodeEnumeration(4, nameof(ResourceNotFound));
        public static RequestErrorCodeEnumeration MethodNotAllowed => new RequestErrorCodeEnumeration(5, nameof(MethodNotAllowed));
        public static RequestErrorCodeEnumeration ConflictResource => new RequestErrorCodeEnumeration(6, nameof(ConflictResource));
        public static RequestErrorCodeEnumeration ConditionNotMet => new RequestErrorCodeEnumeration(7, nameof(ConditionNotMet));
        public static RequestErrorCodeEnumeration MediaTypeNotSupported => new RequestErrorCodeEnumeration(8, nameof(MediaTypeNotSupported));
        public static RequestErrorCodeEnumeration ValidationFailed => new RequestErrorCodeEnumeration(9, nameof(ValidationFailed));
        public static RequestErrorCodeEnumeration PreconditionRequired => new RequestErrorCodeEnumeration(10, nameof(PreconditionRequired));
        public static RequestErrorCodeEnumeration ServerError => new RequestErrorCodeEnumeration(11, nameof(ServerError));
        public static RequestErrorCodeEnumeration IdMismatch => new RequestErrorCodeEnumeration(12, nameof(IdMismatch));

        public RequestErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
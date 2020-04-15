using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Core.Enumerations
{
    public class UserErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static UserErrorCodeEnumeration NotFound => new UserErrorCodeEnumeration(1, nameof(NotFound));
        public static UserErrorCodeEnumeration AlreadyExist => new UserErrorCodeEnumeration(2, nameof(AlreadyExist));
        public static UserErrorCodeEnumeration InsufficientPrivileges => new UserErrorCodeEnumeration(3, nameof(InsufficientPrivileges));
        public static UserErrorCodeEnumeration AnnouncementPreferenceLimitExceeded => new UserErrorCodeEnumeration(4, nameof(AnnouncementPreferenceLimitExceeded));

        private UserErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Core.Enumerations
{
    public class RoleErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static RoleErrorCodeEnumeration NotFound => new RoleErrorCodeEnumeration(1, nameof(NotFound));
        public static RoleErrorCodeEnumeration NameIsAlreadyTaken => new RoleErrorCodeEnumeration(2, nameof(NameIsAlreadyTaken));
        public static RoleErrorCodeEnumeration RolesNotFound => new RoleErrorCodeEnumeration(3, nameof(RolesNotFound));

        private RoleErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
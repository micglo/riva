using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Core.Enumerations
{
    public class StateErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static StateErrorCodeEnumeration NotFound => new StateErrorCodeEnumeration(1, nameof(NotFound));
        public static StateErrorCodeEnumeration NameAlreadyInUse => new StateErrorCodeEnumeration(2, nameof(NameAlreadyInUse));
        public static StateErrorCodeEnumeration PolishNameAlreadyInUse => new StateErrorCodeEnumeration(3, nameof(PolishNameAlreadyInUse));

        private StateErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Enumerations
{
    public class IntegrationEventErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static IntegrationEventErrorCodeEnumeration UnexpectedError => new IntegrationEventErrorCodeEnumeration(1, nameof(UnexpectedError));

        private IntegrationEventErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
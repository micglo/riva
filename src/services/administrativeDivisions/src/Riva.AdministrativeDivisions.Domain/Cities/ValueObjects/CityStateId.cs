using System;
using Riva.AdministrativeDivisions.Domain.Cities.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.Cities.ValueObjects
{
    public class CityStateId
    {
        private readonly Guid _stateId;

        public CityStateId(Guid stateId)
        {
            if (stateId == Guid.Empty || Equals(stateId, new Guid?()) || stateId == new Guid())
                throw new CityStateIdNullException();

            _stateId = stateId;
        }

        public static implicit operator Guid(CityStateId stateId)
        {
            return stateId._stateId;
        }
    }
}
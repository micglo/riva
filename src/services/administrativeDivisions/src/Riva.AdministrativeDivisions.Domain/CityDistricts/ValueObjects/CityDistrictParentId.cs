using System;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects
{
    public class CityDistrictParentId
    {
        private readonly Guid? _parentId;

        public CityDistrictParentId(Guid? parentId)
        {
            if (parentId == Guid.Empty || parentId == new Guid())
                throw new CityDistrictParentIdInvalidValueException();

            _parentId = parentId;
        }

        public static implicit operator Guid?(CityDistrictParentId parentId)
        {
            return parentId._parentId;
        }
    }
}
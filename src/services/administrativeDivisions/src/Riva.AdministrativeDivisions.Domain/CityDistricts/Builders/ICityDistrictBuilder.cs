using System;
using System.Collections.Generic;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Builders
{
    public interface ICityDistrictIdSetter
    {
        ICityDistrictRowVersionSetter SetId(Guid id);
    }

    public interface ICityDistrictRowVersionSetter
    {
        ICityDistrictNameSetter SetRowVersion(IEnumerable<byte> rowVersion);
    }

    public interface ICityDistrictNameSetter
    {
        ICityDistrictPolishNameSetter SetName(string name);
    }

    public interface ICityDistrictPolishNameSetter
    {
        ICityDistrictCityIdSetter SetPolishName(string polishName);
    }

    public interface ICityDistrictCityIdSetter
    {
        ICityDistrictBuilder SetCityId(Guid cityId);
    }

    public interface ICityDistrictBuilder
    {
        ICityDistrictBuilder SetParentId(Guid? parentId);
        ICityDistrictBuilder SetNameVariants(IEnumerable<string> nameVariants);
        CityDistrict Build();
    }
}
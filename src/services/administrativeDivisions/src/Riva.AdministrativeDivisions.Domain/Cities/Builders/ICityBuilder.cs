using System;
using System.Collections.Generic;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;

namespace Riva.AdministrativeDivisions.Domain.Cities.Builders
{
    public interface ICityIdSetter
    {
        ICityRowVersionSetter SetId(Guid id);
    }

    public interface ICityRowVersionSetter
    {
        ICityNameSetter SetRowVersion(IEnumerable<byte> rowVersion);
    }

    public interface ICityNameSetter
    {
        ICityPolishNameSetter SetName(string name);
    }

    public interface ICityPolishNameSetter
    {
        ICityStateIdSetter SetPolishName(string polishName);
    }

    public interface ICityStateIdSetter
    {
        ICityBuilder SetStateId(Guid stateId);
    }

    public interface ICityBuilder
    {
        City Build();
    }
}
using System;
using System.Collections.Generic;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;

namespace Riva.AdministrativeDivisions.Domain.States.Builders
{
    public interface IStateIdSetter
    {
        IStateRowVersionSetter SetId(Guid id);
    }

    public interface IStateRowVersionSetter
    {
        IStateNameSetter SetRowVersion(IEnumerable<byte> rowVersion);
    }

    public interface IStateNameSetter
    {
        IStatePolishNameSetter SetName(string name);
    }

    public interface IStatePolishNameSetter
    {
        IStateBuilder SetPolishName(string polishName);
    }

    public interface IStateBuilder
    {
        State Build();
    }
}
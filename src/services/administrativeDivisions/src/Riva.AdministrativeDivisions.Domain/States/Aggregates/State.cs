using System;
using System.Collections.Generic;
using Riva.AdministrativeDivisions.Domain.States.Builders;
using Riva.AdministrativeDivisions.Domain.States.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.States.Aggregates
{
    public class State : VersionedAggregateBase
    {
        public string Name { get; private set; }
        public string PolishName { get; private set; }

        private State(StateBuilder builder) : base(builder.Id, builder.RowVersion)
        {
            Name = builder.Name;
            PolishName = builder.PolishName;
        }

        public static IStateIdSetter Builder()
        {
            return new StateBuilder();
        }

        public void ChangeName(string name)
        {
            Name = new StateName(name);
        }

        public void ChangePolishName(string polishName)
        {
            PolishName = new StatePolishName(polishName);
        }

        private class StateBuilder : IStateIdSetter, IStateRowVersionSetter, IStateNameSetter, IStatePolishNameSetter, 
            IStateBuilder
        {
            private List<byte> _rowVersion;

            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string PolishName { get; private set; }
            public IReadOnlyCollection<byte> RowVersion => _rowVersion.AsReadOnly();

            public IStateRowVersionSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public IStateNameSetter SetRowVersion(IEnumerable<byte> rowVersion)
            {
                _rowVersion = new VersionedAggregateRowVersion(rowVersion);
                return this;
            }

            public IStatePolishNameSetter SetName(string name)
            {
                Name = new StateName(name);
                return this;
            }

            public IStateBuilder SetPolishName(string polishName)
            {
                PolishName = new StatePolishName(polishName);
                return this;
            }

            public State Build()
            {
                return new State(this);
            }
        }
    }
}
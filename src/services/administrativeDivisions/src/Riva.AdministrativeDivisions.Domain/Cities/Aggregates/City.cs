using System;
using System.Collections.Generic;
using Riva.AdministrativeDivisions.Domain.Cities.Builders;
using Riva.AdministrativeDivisions.Domain.Cities.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.Cities.Aggregates
{
    public class City : VersionedAggregateBase
    {
        public string Name { get; private set; }
        public string PolishName { get; private set; }
        public Guid StateId { get; private set; }

        private City(CityBuilder builder) : base(builder.Id, builder.RowVersion)
        {
            Name = builder.Name;
            PolishName = builder.PolishName;
            StateId = builder.StateId;
        }

        public static ICityIdSetter Builder()
        {
            return new CityBuilder();
        }

        public void ChangeName(string name)
        {
            Name = new CityName(name);
        }

        public void ChangePolishName(string polishName)
        {
            PolishName = new CityPolishName(polishName);
        }

        public void ChangeStateId(Guid stateId)
        {
            StateId = new CityStateId(stateId);
        }

        private class CityBuilder : ICityIdSetter, ICityRowVersionSetter, ICityNameSetter, ICityPolishNameSetter,
            ICityStateIdSetter, ICityBuilder
        {
            private List<byte> _rowVersion;

            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string PolishName { get; private set; }
            public Guid StateId { get; private set; }
            public IReadOnlyCollection<byte> RowVersion => _rowVersion.AsReadOnly();

            public ICityRowVersionSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public ICityNameSetter SetRowVersion(IEnumerable<byte> rowVersion)
            {
                _rowVersion = new VersionedAggregateRowVersion(rowVersion);
                return this;
            }

            public ICityPolishNameSetter SetName(string name)
            {
                Name = new CityName(name);
                return this;
            }

            public ICityStateIdSetter SetPolishName(string polishName)
            {
                PolishName = new CityPolishName(polishName);
                return this;
            }

            public ICityBuilder SetStateId(Guid stateId)
            {
                StateId = new CityStateId(stateId);
                return this;
            }

            public City Build()
            {
                return new City(this);
            }
        }
    }
}
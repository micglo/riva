using System;
using System.Collections.Generic;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Builders;
using Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates
{
    public class CityDistrict : VersionedAggregateBase
    {
        private readonly List<string> _nameVariants;

        public string Name { get; private set; }
        public string PolishName { get; private set; }
        public Guid CityId { get; private set; }
        public Guid? ParentId { get; private set; }
        public IReadOnlyCollection<string> NameVariants => _nameVariants.AsReadOnly();

        private CityDistrict(CityDistrictBuilder builder) : base(builder.Id, builder.RowVersion)
        {
            Name = builder.Name;
            PolishName = builder.PolishName;
            CityId = builder.CityId;
            ParentId = builder.ParentId;
            _nameVariants = builder.NameVariants;
        }
        
        public static ICityDistrictIdSetter Builder()
        {
            return new CityDistrictBuilder();
        }

        public void ChangeName(string name)
        {
            Name = new CityDistrictName(name);
        }

        public void ChangePolishName(string polishName)
        {
            PolishName = new CityDistrictPolishName(polishName);
        }

        public void ChangeCityId(Guid cityId)
        {
            CityId = new CityDistrictCityId(cityId);
        }

        public void ChangeParentId(Guid? parentId)
        {
            ParentId = new CityDistrictParentId(parentId);
        }

        public void AddNameVariant(string nameVariant)
        {
            nameVariant = new CityDistrictNameVariant(nameVariant);
            var anyDuplicates = _nameVariants.Contains(nameVariant);
            if(!anyDuplicates)
                _nameVariants.Add(nameVariant);
        }

        public void RemoveNameVariant(string polishNameVariant)
        {
            _nameVariants.Remove(new CityDistrictNameVariant(polishNameVariant));
        }

        private class CityDistrictBuilder : ICityDistrictIdSetter, ICityDistrictRowVersionSetter, ICityDistrictNameSetter, ICityDistrictPolishNameSetter,
            ICityDistrictCityIdSetter, ICityDistrictBuilder
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string PolishName { get; private set; }
            public Guid? ParentId { get; private set; }
            public Guid CityId { get; private set; }
            public List<byte> RowVersion { get; private set; } = new List<byte>();
            public List<string> NameVariants { get; private set; } = new List<string>();

            public ICityDistrictRowVersionSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public ICityDistrictNameSetter SetRowVersion(IEnumerable<byte> rowVersion)
            {
                RowVersion = new VersionedAggregateRowVersion(rowVersion);
                return this;
            }

            public ICityDistrictPolishNameSetter SetName(string name)
            {
                Name = new CityDistrictName(name);
                return this;
            }

            public ICityDistrictCityIdSetter SetPolishName(string polishName)
            {
                PolishName = new CityDistrictPolishName(polishName);
                return this;
            }

            public ICityDistrictBuilder SetCityId(Guid cityId)
            {
                CityId = new CityDistrictCityId(cityId);
                return this;
            }

            public ICityDistrictBuilder SetParentId(Guid? parentId)
            {
                ParentId = new CityDistrictParentId(parentId);
                return this;
            }

            public ICityDistrictBuilder SetNameVariants(IEnumerable<string> nameVariants)
            {
                NameVariants = new CityDistrictNameVariants(nameVariants);
                return this;
            }

            public CityDistrict Build()
            {
                return new CityDistrict(this);
            }
        }
    }
}
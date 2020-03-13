using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests.CityDistrictRepositoryTests
{
    public class CityDistrictRepositoryTestFixture
    {
        public RivaAdministrativeDivisionsDbContext DbContext { get; }
        public Mock<IMapper> MapperMock { get; }
        public Mock<IOrderByExpressionCreator<CityDistrict>> OrderByExpressionCreatorMock { get; }
        public Mock<IMemoryCache> MemoryCacheMock { get; }
        public ICityDistrictRepository Repository { get; }
        public CityEntity CityEntity { get; }
        public CityDistrict CityDistrict { get; }

        public CityDistrictRepositoryTestFixture(DatabaseFixture fixture)
        {
            DbContext = fixture.DbContext;
            MapperMock = new Mock<IMapper>();
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<CityDistrict>>();
            MemoryCacheMock = new Mock<IMemoryCache>();
            Repository = new CityDistrictRepository(DbContext, MapperMock.Object, OrderByExpressionCreatorMock.Object, MemoryCacheMock.Object);
            CityEntity = InsertCityEntity();
            CityDistrict = InsertCityDistrict("CityDistrictRepositoryTest");
        }

        public CityDistrict InsertCityDistrict(string name)
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32, 64 })
                .SetName(name)
                .SetPolishName(name)
                .SetCityId(CityEntity.Id)
                .SetParentId(Guid.NewGuid())
                .Build();
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = cityDistrict.Id,
                Name = cityDistrict.Name,
                PolishName = cityDistrict.PolishName,
                RowVersion = cityDistrict.RowVersion.ToArray(),
                CityId = cityDistrict.CityId,
                ParentId = cityDistrict.ParentId
            };
            DbContext.CityDistricts.Add(cityDistrictEntity);
            DbContext.SaveChanges();
            return cityDistrict;
        }

        private CityEntity InsertCityEntity()
        {
            var cityEntity = new CityEntity
            {
                Id = Guid.NewGuid(),
                Name = "CityDistrictRepositoryTest",
                PolishName = "CityDistrictRepositoryTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 },
                StateId = Guid.NewGuid()
            };
            DbContext.Cities.Add(cityEntity);
            DbContext.SaveChanges();
            return cityEntity;
        }
    }
}
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests.CityRepositoryTests
{
    public class CityRepositoryTestFixture
    {
        public RivaAdministrativeDivisionsDbContext DbContext { get; }
        public Mock<IMapper> MapperMock { get; }
        public Mock<IOrderByExpressionCreator<City>> OrderByExpressionCreatorMock { get; }
        public Mock<IMemoryCache> MemoryCacheMock { get; }
        public ICityRepository Repository { get; }
        public City City { get; }
        public StateEntity StateEntity { get; }

        public CityRepositoryTestFixture(DatabaseFixture fixture)
        {
            DbContext = fixture.DbContext;
            MapperMock = new Mock<IMapper>();
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<City>>();
            MemoryCacheMock = new Mock<IMemoryCache>();
            Repository = new CityRepository(DbContext, MapperMock.Object, OrderByExpressionCreatorMock.Object, MemoryCacheMock.Object);
            StateEntity = InsertStateEntity();
            City = InsertCity("CityRepositoryTest");
        }

        public City InsertCity(string name)
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(new byte[] { 1, 2, 4, 8, 16, 32, 64 })
                .SetName(name)
                .SetPolishName(name)
                .SetStateId(StateEntity.Id)
                .Build();
            var cityEntity = new CityEntity
            {
                Id = city.Id,
                Name = city.Name,
                PolishName = city.PolishName,
                RowVersion = city.RowVersion.ToArray(),
                StateId = StateEntity.Id
            };
            DbContext.Cities.Add(cityEntity);
            DbContext.SaveChanges();
            return city;
        }

        private StateEntity InsertStateEntity()
        {
            var stateEntity = new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = "CityRepositoryTest",
                PolishName = "CityRepositoryTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 }
            };
            DbContext.States.Add(stateEntity);
            DbContext.SaveChanges();
            return stateEntity;
        }
    }
}
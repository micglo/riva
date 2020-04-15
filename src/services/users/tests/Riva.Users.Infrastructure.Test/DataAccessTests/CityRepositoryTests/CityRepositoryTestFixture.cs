using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Domain.Cities.Repositories;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Repositories;

namespace Riva.Users.Infrastructure.Test.DataAccessTests.CityRepositoryTests
{
    public class CityRepositoryTestFixture
    {
        private readonly RivaUsersDbContext _context;
        public Mock<IMapper> MapperMock { get; }
        public ICityRepository Repository { get; }
        public City City { get; }

        public CityRepositoryTestFixture(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            MapperMock = new Mock<IMapper>();
            Repository = new CityRepository(_context, MapperMock.Object);
            City = InsertCity();
        }

        private City InsertCity()
        {
            var city = new City(Guid.NewGuid(), new List<Guid> { Guid.NewGuid() });
            var cityEntity = new CityEntity
            {
                Id = city.Id,
                CityDistricts = city.CityDistricts.Select(x => new CityDistrictEntity
                {
                    Id = x,
                    CityId = city.Id
                }).ToList()
            };
            _context.Cities.Add(cityEntity);
            _context.SaveChanges();
            return city;
        }
    }
}
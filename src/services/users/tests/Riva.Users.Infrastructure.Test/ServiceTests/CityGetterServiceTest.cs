using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Domain.Cities.Repositories;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class CityGetterServiceTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly ICityGetterService _service;

        public CityGetterServiceTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _service = new CityGetterService(_cityRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Ok_With_City()
        {
            var id = Guid.NewGuid();
            var city = new City(id, new List<Guid> { Guid.NewGuid() });
            var expectedResult = GetResult<City>.Ok(city);

            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(city);

            var result = await _service.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Fail()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var expectedResult = GetResult<City>.Fail(errors);

            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<City>(null));

            var result = await _service.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
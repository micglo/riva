using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.ServiceTests
{
    public class CityVerificationServiceTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly ICityVerificationService _cityVerificationService;

        public CityVerificationServiceTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _cityVerificationService = new CityVerificationService(_cityRepositoryMock.Object);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_True()
        {
            const string name = "Name";
            var expectedResult = VerificationResult.Ok();

            _cityRepositoryMock.Setup(x => x.GetByNameAndStateIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult<City>(null));

            var result = await _cityVerificationService.VerifyNameIsNotTakenAsync(name, Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_False_And_Errors_When_Name_Is_Already_Taken()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var expectedResult = VerificationResult.Fail(new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NameAlreadyInUse, CityErrorMessage.NameAlreadyInUse)
            });

            _cityRepositoryMock.Setup(x => x.GetByNameAndStateIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(city);

            var result = await _cityVerificationService.VerifyNameIsNotTakenAsync(city.Name, city.StateId);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyPolishNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_True()
        {
            const string polishName = "PolishName";
            var expectedResult = VerificationResult.Ok();

            _cityRepositoryMock.Setup(x => x.GetByPolishNameAndStateIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult<City>(null));

            var result = await _cityVerificationService.VerifyPolishNameIsNotTakenAsync(polishName, Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyPolishNameIsNotTakenAsync_Should_Return_VerificationResult_With_Success_False_And_Errors_When_PolishName_Is_Already_Taken()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var expectedResult = VerificationResult.Fail(new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.PolishNameAlreadyInUse, CityErrorMessage.PolishNameAlreadyInUse)
            });

            _cityRepositoryMock.Setup(x => x.GetByPolishNameAndStateIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(city);

            var result = await _cityVerificationService.VerifyPolishNameIsNotTakenAsync(city.PolishName, city.StateId);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
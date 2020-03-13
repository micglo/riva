using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.ServiceTests
{
    public class CityDistrictVerificationServiceTest
    {
        private readonly Mock<ICityDistrictRepository> _cityDistrictRepositoryMock;
        private readonly Mock<ICityDistrictGetterService> _cityDistrictGetterServiceMock;
        private readonly ICityDistrictVerificationService _service;

        public CityDistrictVerificationServiceTest()
        {
            _cityDistrictRepositoryMock = new Mock<ICityDistrictRepository>();
            _cityDistrictGetterServiceMock = new Mock<ICityDistrictGetterService>();
            _service = new CityDistrictVerificationService(_cityDistrictRepositoryMock.Object, _cityDistrictGetterServiceMock.Object);
        }

        [Fact]
        public async Task VerifyParentExistsAsync_Should_Return_Success_VerificationResult()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .Build();
            var getCityDistrictResult = GetResult<CityDistrict>.Ok(cityDistrict);
            var expectedResult = VerificationResult.Ok();

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);

            var result = await _service.VerifyParentExistsAsync(cityDistrict.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyParentExistsAsync_Should_Return_Fail_VerificationResult_When_Parent_Is_Not_Found()
        {
            var getCityDistrictErrors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var getCityDistrictResult = GetResult<CityDistrict>.Fail(getCityDistrictErrors);
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.ParentNotFound, CityDistrictErrorMessage.ParentNotFound)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);

            var result = await _service.VerifyParentExistsAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_Success_VerificationResult()
        {
            var expectedResult = VerificationResult.Ok();

            _cityDistrictRepositoryMock.Setup(x => x.GetByNameAndCityIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult<CityDistrict>(null));

            var result = await _service.VerifyNameIsNotTakenAsync("Name", Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyNameIsNotTakenAsync_Should_Return_Fail_VerificationResult_When_CityDistrict_With_Given_Name_Already_Exist()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .Build();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NameAlreadyInUse,
                    CityDistrictErrorMessage.NameAlreadyInUse)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _cityDistrictRepositoryMock.Setup(x => x.GetByNameAndCityIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(cityDistrict);

            var result = await _service.VerifyNameIsNotTakenAsync("Name", Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyPolishNameIsNotTakenAsync_Should_Return_Success_VerificationResult()
        {
            var expectedResult = VerificationResult.Ok();

            _cityDistrictRepositoryMock.Setup(x => x.GetByPolishNameAndCityIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult<CityDistrict>(null));

            var result = await _service.VerifyPolishNameIsNotTakenAsync("PolishName", Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyPolishNameIsNotTakenAsync_Should_Return_Fail_VerificationResult_When_CityDistrict_With_Given_Name_Already_Exist()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .Build();
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.PolishNameAlreadyInUse,
                    CityDistrictErrorMessage.PolishNameAlreadyInUse)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _cityDistrictRepositoryMock.Setup(x => x.GetByPolishNameAndCityIdAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(cityDistrict);

            var result = await _service.VerifyPolishNameIsNotTakenAsync("PolishName", Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
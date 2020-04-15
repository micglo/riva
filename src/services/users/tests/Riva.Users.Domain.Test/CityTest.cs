using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Domain.Cities.Exceptions;
using Xunit;

namespace Riva.Users.Domain.Test
{
    public class CityTest
    {
        [Fact]
        public void Should_Create_City()
        {
            var id = Guid.NewGuid();
            var cityDistricts = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            var city = new City(id, cityDistricts);

            city.Should().NotBeNull();
            city.Id.Should().Be(id);
            city.CityDistricts.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public void Should_Throw_CityCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            Action result = () =>
            {
                var unused = new City(Guid.NewGuid(), null);
            };

            result.Should().ThrowExactly<CityCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_CityCityDistrictsInvalidsValueException_When_CityDistricts_Contains_Empty_Guid()
        {
            var cityDistricts = new List<Guid> { Guid.Empty };

            Action result = () =>
            {
                var unused = new City(Guid.NewGuid(), cityDistricts);
            };

            result.Should().ThrowExactly<CityCityDistrictsInvalidsValueException>()
                .WithMessage("CityDistricts argument is invalid.");
        }

        [Fact]
        public void Should_Throw_CityCityDistrictsInvalidsValueException_When_CityDistricts_Contains_New_Guid()
        {
            var cityDistricts = new List<Guid> { new Guid() };

            Action result = () =>
            {
                var unused = new City(Guid.NewGuid(), cityDistricts);
            };

            result.Should().ThrowExactly<CityCityDistrictsInvalidsValueException>()
                .WithMessage("CityDistricts argument is invalid.");
        }

        [Fact]
        public void Should_Throw_CityCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicated_Values()
        {
            var cityDistrict = Guid.NewGuid();
            var cityDistricts = new List<Guid>
            {
                cityDistrict,
                cityDistrict
            };

            Action result = () =>
            {
                var unused = new City(Guid.NewGuid(), cityDistricts);
            };

            result.Should().ThrowExactly<CityCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }
    }
}
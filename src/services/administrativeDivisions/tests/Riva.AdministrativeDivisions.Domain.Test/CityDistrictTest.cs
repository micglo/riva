using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;
using Xunit;

namespace Riva.AdministrativeDivisions.Domain.Test
{
    public class CityDistrictTest
    {
        [Fact]
        public void Should_Create_CityDistrict()
        {
            var result = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(null)
                .Build();

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_CityDistrictNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(name)
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_CityDistrictPolishNameNullException_When_PolishName_Is_Null_Or_Empty(string polishName)
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName(polishName)
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictPolishNameNullException>()
                .WithMessage("PolishName argument is required.");
        }

        [Fact]
        public void Should_Throw_CityDistrictNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(name)
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void Should_Throw_CityDistrictPolishNameMaxLengthException_When_PolishName_Exceed_Allowed_Max_Length_Value()
        {
            var polishName = CreateString(257);
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName(polishName)
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictPolishNameMaxLengthException>()
                .WithMessage("PolishName argument max length is 256.");
        }

        [Fact]
        public void Should_Throw_CityDistrictCityIdNullException_When_CityId_Is_Guid_Empty()
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.Empty)
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void Should_Throw_CityDistrictCityIdNullException_When_CityId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(new Guid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void Should_Throw_CityDistrictParentInvalidValueException_When_ParentId_Is_Guid_Empty()
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(Guid.Empty)
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictParentIdInvalidValueException>()
                .WithMessage("ParentId argument is invalid.");
        }

        [Fact]
        public void Should_Throw_CityDistrictParentInvalidValueException_When_ParentId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant" })
                    .SetParentId(new Guid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictParentIdInvalidValueException>()
                .WithMessage("ParentId argument is invalid.");
        }

        [Fact]
        public void Should_Throw_CityDistrictNameVariantsNullException_When_NameVariants_Is_Null()
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(null)
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictNameVariantsNullException>()
                .WithMessage("NameVariants argument is required.");
        }

        [Theory]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_CityDistrictNameVariantsInvalidValueException_When_NameVariants_Contains_Null_Or_Empty_Value(string nameVariant)
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string>{ nameVariant })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictNameVariantsInvalidValueException>()
                .WithMessage("NameVariants argument is invalid.");
        }

        [Fact]
        public void Should_Throw_CityDistrictNameVariantsDuplicatedValuesException_When_NameVariants_Contains_Duplicated_Values()
        {
            Action result = () =>
            {
                var unused = CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .SetNameVariants(new List<string> { "NameVariant", "NameVariant" })
                    .SetParentId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityDistrictNameVariantsDuplicatedValuesException>()
                .WithMessage("NameVariants argument contains duplicate values.");
        }

        [Fact]
        public void ChangeName_Should_Change_Name()
        {
            const string name = "NewName";
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            cityDistrict.ChangeName(name);

            cityDistrict.Name.Should().BeEquivalentTo(name);
        }

        [Fact]
        public void ChangePolishName_Should_Change_PolishName()
        {
            const string polishName = "NewPolishName";
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            cityDistrict.ChangePolishName(polishName);

            cityDistrict.PolishName.Should().BeEquivalentTo(polishName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangeName_Should_Throw_CityDistrictNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangeName(name);

            result.Should().ThrowExactly<CityDistrictNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangePolishName_Should_Throw_CityDistrictPolishNameNullException_When_PolishName_Is_Null_Or_Empty(string polishName)
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangePolishName(polishName);

            result.Should().ThrowExactly<CityDistrictPolishNameNullException>()
                .WithMessage("PolishName argument is required.");
        }

        [Fact]
        public void ChangeName_Should_Throw_CityDistrictNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangeName(name);

            result.Should().ThrowExactly<CityDistrictNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void ChangePolishName_Should_Throw_CityDistrictPolishNameMaxLengthException_When_PolishName_Exceed_Allowed_Max_Length_Value()
        {
            var polishName = CreateString(257);
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangePolishName(polishName);

            result.Should().ThrowExactly<CityDistrictPolishNameMaxLengthException>()
                .WithMessage("PolishName argument max length is 256.");
        }

        [Fact]
        public void ChangeCityId_Should_Change_CityId()
        {
            var cityId = Guid.NewGuid();
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            cityDistrict.ChangeCityId(cityId);

            cityDistrict.CityId.Should().Be(cityId);
        }

        [Fact]
        public void ChangeCityId_Should_Throw_CityDistrictCityIdNullException_When_CityId_Is_New_Guid()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangeCityId(new Guid());

            result.Should().ThrowExactly<CityDistrictCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void ChangeCityId_Should_Throw_CityDistrictCityIdNullException_When_CityId_Is_Empty_Guid()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangeCityId(Guid.Empty);

            result.Should().ThrowExactly<CityDistrictCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void ChangeParentId_Should_Change_ParentId()
        {
            var parentId = Guid.NewGuid();
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            cityDistrict.ChangeParentId(parentId);

            cityDistrict.ParentId.Should().Be(parentId);
        }

        [Fact]
        public void ChangeParentId_Should_Throw_CityDistrictCityIdNullException_When_ParentId_Is_New_Guid()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangeParentId(new Guid());

            result.Should().ThrowExactly<CityDistrictParentIdInvalidValueException>()
                .WithMessage("ParentId argument is invalid.");
        }

        [Fact]
        public void ChangeParentId_Should_Throw_CityDistrictCityIdNullException_When_ParentId_Is_Empty_Guid()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.ChangeParentId(Guid.Empty);

            result.Should().ThrowExactly<CityDistrictParentIdInvalidValueException>()
                .WithMessage("ParentId argument is invalid.");
        }

        [Fact]
        public void AddNameVariant_Should_Add_NameVariant()
        {
            const string nameVariant = "NameVariant";
            const string nameVariantToAdd = "NameVariantToAdd";
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string>{ nameVariant })
                .SetParentId(Guid.NewGuid())
                .Build();

            cityDistrict.AddNameVariant(nameVariantToAdd);

            cityDistrict.NameVariants.Should().BeEquivalentTo(new Collection<string> { nameVariant, nameVariantToAdd });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void AddNameVariant_Should_Throw_CityDistrictNameVariantNullException_When_NameVariant_Is_Null_Or_Empty(string nameVariantToAdd)
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.AddNameVariant(nameVariantToAdd);

            result.Should().ThrowExactly<CityDistrictNameVariantNullException>()
                .WithMessage("NameVariant argument is required.");
        }

        [Fact]
        public void RemoveNameVariant_Should_Remove_NameVariant()
        {
            const string nameVariant = "NameVariant";
            const string nameVariantToRemove = "NewNameVariantToRemove";
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { nameVariant, nameVariantToRemove })
                .SetParentId(Guid.NewGuid())
                .Build();

            cityDistrict.RemoveNameVariant(nameVariantToRemove);

            cityDistrict.NameVariants.Should().BeEquivalentTo(new Collection<string> { nameVariant });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void RemoveNameVariant_Should_Throw_CityDistrictNameVariantNullException_When_NameVariant_Is_Null_Or_Empty(string nameVariantToRemove)
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetNameVariants(new List<string> { "NameVariant" })
                .SetParentId(Guid.NewGuid())
                .Build();

            Action result = () => cityDistrict.RemoveNameVariant(nameVariantToRemove);

            result.Should().ThrowExactly<CityDistrictNameVariantNullException>()
                .WithMessage("NameVariant argument is required.");
        }

        private static string CreateString(int charNumber)
        {
            var secretHash = string.Empty;
            for (var i = 0; i < charNumber; i++)
            {
                secretHash += "a";
            }

            return secretHash;
        }
    }
}
using System;
using FluentAssertions;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Exceptions;
using Xunit;

namespace Riva.AdministrativeDivisions.Domain.Test
{
    public class CityTest
    {
        [Fact]
        public void Should_Create_City()
        {
            var result = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_CityNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            Action result = () =>
            {
                var unused = City.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(name)
                    .SetPolishName("PolishName")
                    .SetStateId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_CityPolishNameNullException_When_PolishName_Is_Null_Or_Empty(string polishName)
        {
            Action result = () =>
            {
                var unused = City.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName(polishName)
                    .SetStateId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityPolishNameNullException>()
                .WithMessage("PolishName argument is required.");
        }

        [Fact]
        public void Should_Throw_CityNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            Action result = () =>
            {
                var unused = City.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(name)
                    .SetPolishName("PolishName")
                    .SetStateId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void Should_Throw_CityPolishNameMaxLengthException_When_PolishName_Exceed_Allowed_Max_Length_Value()
        {
            var polishName = CreateString(257);
            Action result = () =>
            {
                var unused = City.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName(polishName)
                    .SetStateId(Guid.NewGuid())
                    .Build();
            };

            result.Should().ThrowExactly<CityPolishNameMaxLengthException>()
                .WithMessage("PolishName argument max length is 256.");
        }

        [Fact]
        public void Should_Throw_CityStateIdNullException_When_StateId_Is_Guid_Empty()
        {
            Action result = () =>
            {
                var unused = City.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetStateId(Guid.Empty)
                    .Build();
            };

            result.Should().ThrowExactly<CityStateIdNullException>()
                .WithMessage("StateId argument is required.");
        }

        [Fact]
        public void Should_Throw_CityStateIdNullException_When_StateId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = City.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetStateId(new Guid())
                    .Build();
            };

            result.Should().ThrowExactly<CityStateIdNullException>()
                .WithMessage("StateId argument is required.");
        }

        [Fact]
        public void ChangeName_Should_Change_Name()
        {
            const string name = "NewName";
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            city.ChangeName(name);

            city.Name.Should().BeEquivalentTo(name);
        }

        [Fact]
        public void ChangePolishName_Should_Change_PolishName()
        {
            const string polishName = "NewPolishName";
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            city.ChangePolishName(polishName);

            city.PolishName.Should().BeEquivalentTo(polishName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangeName_Should_Throw_CityNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            Action result = () => city.ChangeName(name);

            result.Should().ThrowExactly<CityNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangePolishName_Should_Throw_CityPolishNameNullException_When_PolishName_Is_Null_Or_Empty(string polishName)
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            Action result = () => city.ChangePolishName(polishName);

            result.Should().ThrowExactly<CityPolishNameNullException>()
                .WithMessage("PolishName argument is required.");
        }

        [Fact]
        public void ChangeName_Should_Throw_CityNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            Action result = () => city.ChangeName(name);

            result.Should().ThrowExactly<CityNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void ChangePolishName_Should_Throw_CityPolishNameMaxLengthException_When_PolishName_Exceed_Allowed_Max_Length_Value()
        {
            var polishName = CreateString(257);
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            Action result = () => city.ChangePolishName(polishName);

            result.Should().ThrowExactly<CityPolishNameMaxLengthException>()
                .WithMessage("PolishName argument max length is 256.");
        }

        [Fact]
        public void ChangeStateId_Should_Change_StateId()
        {
            var stateId = Guid.NewGuid();
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            city.ChangeStateId(stateId);

            city.StateId.Should().Be(stateId);
        }

        [Fact]
        public void ChangeStateId_Should_Throw_CityStateIdNullException_When_StateId_Is_New_Guid()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            Action result = () => city.ChangeStateId(new Guid());

            result.Should().ThrowExactly<CityStateIdNullException>()
                .WithMessage("StateId argument is required.");
        }

        [Fact]
        public void ChangeStateId_Should_Throw_CityStateIdNullException_When_StateId_Is_Empty_Guid()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            Action result = () => city.ChangeStateId(Guid.Empty);

            result.Should().ThrowExactly<CityStateIdNullException>()
                .WithMessage("StateId argument is required.");
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
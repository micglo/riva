using System;
using FluentAssertions;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Exceptions;
using Xunit;

namespace Riva.AdministrativeDivisions.Domain.Test
{
    public class StateTest
    {
        [Fact]
        public void Should_Create_State()
        {
            var result = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_StateNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            Action result = () =>
            {
                var unused = State.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(name)
                    .SetPolishName("PolishName")
                    .Build();
            };

            result.Should().ThrowExactly<StateNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_StatePolishNameNullException_When_PolishName_Is_Null_Or_Empty(string polishName)
        {
            Action result = () =>
            {
                var unused = State.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName(polishName)
                    .Build();
            };

            result.Should().ThrowExactly<StatePolishNameNullException>()
                .WithMessage("PolishName argument is required.");
        }

        [Fact]
        public void Should_Throw_StateNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            Action result = () =>
            {
                var unused = State.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(name)
                    .SetPolishName("PolishName")
                    .Build();
            };

            result.Should().ThrowExactly<StateNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void Should_Throw_StatePolishNameMaxLengthException_When_PolishName_Exceed_Allowed_Max_Length_Value()
        {
            var polishName = CreateString(257);
            Action result = () =>
            {
                var unused = State.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName(polishName)
                    .Build();
            };

            result.Should().ThrowExactly<StatePolishNameMaxLengthException>()
                .WithMessage("PolishName argument max length is 256.");
        }

        [Fact]
        public void ChangeName_Should_Change_Name()
        {
            const string name = "NewName";
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            state.ChangeName(name);

            state.Name.Should().BeEquivalentTo(name);
        }

        [Fact]
        public void ChangePolishName_Should_Change_PolishName()
        {
            const string polishName = "NewPolishName";
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            state.ChangePolishName(polishName);

            state.PolishName.Should().BeEquivalentTo(polishName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangeName_Should_Throw_StateNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            Action result = () => state.ChangeName(name);

            result.Should().ThrowExactly<StateNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangePolishName_Should_Throw_StatePolishNameNullException_When_PolishName_Is_Null_Or_Empty(string polishName)
        {
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            Action result = () => state.ChangePolishName(polishName);

            result.Should().ThrowExactly<StatePolishNameNullException>()
                .WithMessage("PolishName argument is required.");
        }

        [Fact]
        public void ChangeName_Should_Throw_StateNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            Action result = () => state.ChangeName(name);

            result.Should().ThrowExactly<StateNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void ChangePolishName_Should_Throw_StatePolishNameMaxLengthException_When_PolishName_Exceed_Allowed_Max_Length_Value()
        {
            var polishName = CreateString(257);
            var state = State.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .Build();

            Action result = () => state.ChangePolishName(polishName);

            result.Should().ThrowExactly<StatePolishNameMaxLengthException>()
                .WithMessage("PolishName argument max length is 256.");
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
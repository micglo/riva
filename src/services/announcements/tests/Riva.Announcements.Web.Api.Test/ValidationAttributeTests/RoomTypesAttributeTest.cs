using System.Collections.Generic;
using FluentAssertions;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.Announcements.Web.Api.ValidationAttributes;
using Riva.BuildingBlocks.WebApiTest.ValidationAttributeTests;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.ValidationAttributeTests
{
    public class RoomTypesAttributeTest : IClassFixture<ValidationAttributeTestFixture>
    {
        private readonly ValidationAttributeTestFixture _fixture;

        public RoomTypesAttributeTest(ValidationAttributeTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Should_Be_Valid_When_Target_Contains_RoomTypes()
        {
            var target = new Target
            {
                RoomTypes = new List<RoomType> { RoomType.Single, RoomType.Double }
            };

            var result = _fixture.TestExecutor.Execute(target);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Should_Be_Valid_When_Target_Contains_Empty_Collection()
        {
            var target = new Target
            {
                RoomTypes = new List<RoomType>()
            };

            var result = _fixture.TestExecutor.Execute(target);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Should_Be_InValid_When_Target_Contains_Duplicated_Values()
        {
            var target = new Target
            {
                RoomTypes = new List<RoomType> { RoomType.Single, RoomType.Single }
            };
            var expectedError = $"The field {nameof(Target.RoomTypes)} cannot have duplicated values.";

            var result = _fixture.TestExecutor.Execute(target);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(expectedError).And.HaveCount(1);
        }

        private class Target
        {
            [RoomTypes]
            public IEnumerable<RoomType> RoomTypes { get; set; }
        }
    }
}
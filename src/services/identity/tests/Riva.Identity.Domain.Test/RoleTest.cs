using System;
using FluentAssertions;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Exceptions;
using Xunit;

namespace Riva.Identity.Domain.Test
{
    public class RoleTest
    {
        [Fact]
        public void Should_Create_Role()
        {
            var id = Guid.NewGuid();
            var rowVersion = Array.Empty<byte>();
            const string name = "Name";
            var result = new Role(id, rowVersion, name);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.RowVersion.Should().BeEquivalentTo(rowVersion);
            result.Name.Should().Be(name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_RoleNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            Action result = () =>
            {
                var unused = new Role(Guid.NewGuid(), Array.Empty<byte>(), name);
            };

            result.Should().ThrowExactly<RoleNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Fact]
        public void Should_Throw_RoleNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            Action result = () =>
            {
                var unused = new Role(Guid.NewGuid(), Array.Empty<byte>(), name);
            };

            result.Should().ThrowExactly<RoleNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
        }

        [Fact]
        public void ChangeName_Should_Change_Name()
        {
            const string name = "NewName";
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), "Name");
            
            role.ChangeName(name);

            role.Name.Should().BeEquivalentTo(name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangeName_Should_Throw_RoleNameNullException_When_Name_Is_Null_Or_Empty(string name)
        {
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), "Name");
            
            Action result = () => role.ChangeName(name);

            result.Should().ThrowExactly<RoleNameNullException>()
                .WithMessage("Name argument is required.");
        }

        [Fact]
        public void ChangeName_Should_Throw_RoleNameMaxLengthException_When_Name_Exceed_Allowed_Max_Length_Value()
        {
            var name = CreateString(257);
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), "Name");
            
            Action result = () => role.ChangeName(name);

            result.Should().ThrowExactly<RoleNameMaxLengthException>()
                .WithMessage("Name argument max length is 256.");
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
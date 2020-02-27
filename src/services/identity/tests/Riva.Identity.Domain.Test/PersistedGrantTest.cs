using System;
using FluentAssertions;
using Riva.Identity.Domain.PersistedGrants.Aggregates;
using Riva.Identity.Domain.PersistedGrants.Exceptions;
using Xunit;

namespace Riva.Identity.Domain.Test
{
    public class PersistedGrantTest
    {
        [Fact]
        public void Should_Create_PersistedGrant()
        {
            const string key = "key";
            const string type = "type";
            var subjectId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var creationTime = DateTime.UtcNow;
            const string data = "data";
            var expiration = DateTime.UtcNow.AddDays(1);

            var result = PersistedGrant.Builder()
                .SetKey(key)
                .SetType(type)
                .SetSubjectId(subjectId)
                .SetClientId(clientId)
                .SetCreationTime(creationTime)
                .SetData(data)
                .SetExpiration(expiration)
                .Build();

            result.Should().NotBeNull();
            result.Key.Should().Be(key);
            result.Type.Should().Be(type);
            result.SubjectId.Should().Be(subjectId);
            result.ClientId.Should().Be(clientId);
            result.CreationTime.Should().Be(creationTime);
            result.Data.Should().Be(data);
            result.Expiration.Should().Be(expiration);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Throw_PersistedGrantKeyNullException_When_Key_Is_Null_Or_Empty(string key)
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey(key)
                    .SetType("type")
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantKeyNullException>()
                .WithMessage("Key argument is required.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Throw_PersistedGrantTypeNullException_When_Key_Is_Null_Or_Empty(string type)
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType(type)
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantTypeNullException>()
                .WithMessage("Type argument is required.");
        }

        [Fact]
        public void Should_Throw_PersistedGrantSubjectIdNullException_When_SubjectId_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(Guid.Empty)
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantSubjectIdNullException>()
                .WithMessage("SubjectId argument is required.");
        }

        [Fact]
        public void Should_Throw_PersistedGrantSubjectIdNullException_When_SubjectId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(new Guid())
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantSubjectIdNullException>()
                .WithMessage("SubjectId argument is required.");
        }

        [Fact]
        public void Should_Throw_PersistedGrantClientIdNullException_When_ClientId_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(Guid.Empty)
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantClientIdNullException>()
                .WithMessage("ClientId argument is required.");
        }

        [Fact]
        public void Should_Throw_PersistedGrantClientIdNullException_When_ClientId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(new Guid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantClientIdNullException>()
                .WithMessage("ClientId argument is required.");
        }

        [Fact]
        public void Should_Throw_PersistedGrantCreationTimeMinValueException_When_CreationTime_Is_Min()
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.MinValue)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantCreationTimeMinValueException>()
                .WithMessage("CreationTime argument cannot be min value.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Throw_PersistedGrantDataNullException_When_Key_Is_Null_Or_Empty(string data)
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData(data)
                    .SetExpiration(DateTime.UtcNow.AddDays(1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantDataNullException>()
                .WithMessage("Data argument is required.");
        }

        [Fact]
        public void Should_Throw_PersistedGrantExpirationMinValueException_When_Expiration_Is_Earlier_Than_CreationTime()
        {
            Action result = () =>
            {
                var unused = PersistedGrant.Builder()
                    .SetKey("key")
                    .SetType("type")
                    .SetSubjectId(Guid.NewGuid())
                    .SetClientId(Guid.NewGuid())
                    .SetCreationTime(DateTime.UtcNow)
                    .SetData("data")
                    .SetExpiration(DateTime.UtcNow.AddDays(-1))
                    .Build();
            };

            result.Should().ThrowExactly<PersistedGrantExpirationMinValueException>()
                .WithMessage("Expiration argument cannot be earlier than CreationTime.");
        }
    }
}
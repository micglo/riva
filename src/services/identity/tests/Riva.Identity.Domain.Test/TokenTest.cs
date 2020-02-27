using System;
using FluentAssertions;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions;
using Xunit;

namespace Riva.Identity.Domain.Test
{
    public class TokenTest
    {
        [Fact]
        public void Should_Create_Token()
        {
            var issued = DateTimeOffset.UtcNow;
            var expires = DateTimeOffset.UtcNow.AddDays(1);
            var type = TokenTypeEnumeration.AccountConfirmation;
            const string value = "value";

            var result = Token.Builder()
                .SetIssued(issued)
                .SetExpires(expires)
                .SetType(type)
                .SetValue(value)
                .Build();

            result.Should().NotBeNull();
            result.Issued.Should().Be(issued);
            result.Expires.Should().Be(expires);
            result.Type.Should().Be(type);
            result.Value.Should().Be(value);
        }

        [Fact]
        public void Should_Throw_TokenIssuedMinValueException_When_Issued_Is_Min_Value()
        {
            Action result = () =>
            {
                var unused = Token.Builder()
                    .SetIssued(DateTimeOffset.MinValue)
                    .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                    .SetType(TokenTypeEnumeration.AccountConfirmation)
                    .SetValue("value")
                    .Build();
            };

            result.Should().ThrowExactly<TokenIssuedMinValueException>()
                .WithMessage("Issued argument cannot be min value.");
        }

        [Fact]
        public void Should_Throw_TokenExpiresMinValueException_When_Expires_Is_Equal_Issued()
        {
            var issued = DateTimeOffset.UtcNow;
            Action result = () =>
            {
                var unused = Token.Builder()
                    .SetIssued(issued)
                    .SetExpires(issued)
                    .SetType(TokenTypeEnumeration.AccountConfirmation)
                    .SetValue("value")
                    .Build();
            };

            result.Should().ThrowExactly<TokenExpiresMinValueException>()
                .WithMessage("Expires argument must be greater than Issued argument.");
        }

        [Fact]
        public void Should_Throw_TokenExpiresMinValueException_When_Expires_Is_Lower_Than_Issued()
        {
            var issued = DateTimeOffset.UtcNow;
            Action result = () =>
            {
                var unused = Token.Builder()
                    .SetIssued(issued)
                    .SetExpires(issued.AddDays(-1))
                    .SetType(TokenTypeEnumeration.AccountConfirmation)
                    .SetValue("value")
                    .Build();
            };

            result.Should().ThrowExactly<TokenExpiresMinValueException>()
                .WithMessage("Expires argument must be greater than Issued argument.");
        }

        [Fact]
        public void Should_Throw_TokenTypeNullException_When_Type_Is_Null()
        {
            Action result = () =>
            {
                var unused = Token.Builder()
                    .SetIssued(DateTimeOffset.UtcNow)
                    .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                    .SetType(null)
                    .SetValue("value")
                    .Build();
            };

            result.Should().ThrowExactly<TokenTypeNullException>()
                .WithMessage("TokenType argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_TokenValueNullException_When_Value_Is_Null_Or_Empty(string value)
        {
            Action result = () =>
            {
                var unused = Token.Builder()
                    .SetIssued(DateTimeOffset.UtcNow)
                    .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                    .SetType(TokenTypeEnumeration.AccountConfirmation)
                    .SetValue(value)
                    .Build();
            };

            result.Should().ThrowExactly<TokenValueNullException>()
                .WithMessage("Value argument is required.");
        }
    }
}
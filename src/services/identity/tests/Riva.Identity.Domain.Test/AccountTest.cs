using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Events;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;
using Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions;
using Xunit;

namespace Riva.Identity.Domain.Test
{
    public class AccountTest
    {
        [Fact]
        public void Should_Create_Account()
        {
            var issued = DateTimeOffset.UtcNow;
            var expires = issued.AddDays(1);
            var tokenType = TokenTypeEnumeration.AccountConfirmation;
            const string tokenValue = "value";
            var token = Token.Builder()
                .SetIssued(issued)
                .SetExpires(expires)
                .SetType(tokenType)
                .SetValue(tokenValue)
                .Build();

            var accountId = Guid.NewGuid();
            const string email = "email@email.com";
            const bool confirmed = false;
            const string passwordHash = "PasswordHash";
            var securityStamp = Guid.NewGuid();
            var created = DateTimeOffset.UtcNow;
            var roles = new List<Guid> { Guid.NewGuid() };
            var lastLogin = DateTimeOffset.UtcNow;
            var tokens = new List<Token> { token };
            var result = Account.Builder()
                .SetId(accountId)
                .SetEmail(email)
                .SetConfirmed(confirmed)
                .SetPasswordHash(passwordHash)
                .SetSecurityStamp(securityStamp)
                .SetCreated(created)
                .SetRoles(roles)
                .SetLastLogin(lastLogin)
                .SetTokens(tokens)
                .Build();

            result.Should().NotBeNull();
            result.Id.Should().Be(accountId);
            result.Email.Should().Be(email);
            result.Confirmed.Should().Be(confirmed);
            result.PasswordHash.Should().Be(passwordHash);
            result.SecurityStamp.Should().Be(securityStamp);
            result.Created.Should().Be(created);
            result.Roles.Should().BeEquivalentTo(roles);
            result.LastLogin.Should().Be(lastLogin);
            result.Tokens.Should().BeEquivalentTo(tokens);
        }

        [Fact]
        public void Should_Create_Account_With_Empty_Roles_And_Tokens()
        {
            var accountId = Guid.NewGuid();
            const string email = "email@email.com";
            const bool confirmed = false;
            const string passwordHash = "PasswordHash";
            var securityStamp = Guid.NewGuid();
            var created = DateTimeOffset.UtcNow;
            var result = Account.Builder()
                .SetId(accountId)
                .SetEmail(email)
                .SetConfirmed(confirmed)
                .SetPasswordHash(passwordHash)
                .SetSecurityStamp(securityStamp)
                .SetCreated(created)
                .Build();

            result.Should().NotBeNull();
            result.Id.Should().Be(accountId);
            result.Email.Should().Be(email);
            result.Confirmed.Should().Be(confirmed);
            result.PasswordHash.Should().Be(passwordHash);
            result.SecurityStamp.Should().Be(securityStamp);
            result.Created.Should().Be(created);
            result.Roles.Should().BeEmpty();
            result.LastLogin.Should().BeNull();
            result.Roles.Should().BeEmpty();
            result.Tokens.Should().BeEmpty();
        }

        [Fact]
        public void Should_Throw_AccountEmailFormatException_When_Email_Is_In_Incorrect_Format()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .Build();
            };

            result.Should().ThrowExactly<AccountEmailFormatException>()
                .WithMessage("Email argument is not in the form required for an e-mail address.");
        }

        [Fact]
        public void Should_Throw_AccountEmailMaxLengthException_When_Email_Exceed_Allowed_Max_Length_Value()
        {
            var email = CreateString(257) + "@email.com";
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail(email)
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .Build();
            };

            result.Should().ThrowExactly<AccountEmailMaxLengthException>()
                .WithMessage("Email argument max length is 256.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_AccountEmailNullException_When_Email_Is_Null_Or_Empty(string email)
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail(email)
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .Build();
            };

            result.Should().ThrowExactly<AccountEmailNullException>()
                .WithMessage("Email argument is required.");
        }

        [Fact]
        public void Should_Throw_AccountSecurityStampInvalidValueException_When_SecurityStamp_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(new Guid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .Build();
            };

            result.Should().ThrowExactly<AccountSecurityStampNullException>()
                .WithMessage("SecurityStamp argument is required.");
        }

        [Fact]
        public void Should_Throw_AccountSecurityStampInvalidValueException_When_SecurityStamp_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.Empty)
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .Build();
            };

            result.Should().ThrowExactly<AccountSecurityStampNullException>()
                .WithMessage("SecurityStamp argument is required.");
        }

        [Fact]
        public void Should_Throw_AccountRolesNullException_When_Roles_Is_Null()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(null)
                    .Build();
            };

            result.Should().ThrowExactly<AccountRolesNullException>()
                .WithMessage("Roles argument is required.");
        }

        [Fact]
        public void Should_Throw_AccountRolesInvalidException_When_Roles_Contains_New_Guid_Or_Empty_Guid_Values()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid> { Guid.Empty, new Guid() })
                    .Build();
            };

            result.Should().ThrowExactly<AccountRolesInvalidException>()
                .WithMessage("Roles argument is invalid.");
        }

        [Fact]
        public void Should_Throw_AccountRolesDuplicateValuesException_When_Roles_Contains_Duplicated_Values()
        {
            var roleId = Guid.NewGuid();
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid> { roleId, roleId })
                    .Build();
            };

            result.Should().ThrowExactly<AccountRolesDuplicateValuesException>()
                .WithMessage("Roles argument contains duplicate values.");
        }

        [Fact]
        public void Should_Throw_AccountTokensNullException_When_Tokens_Is_Null()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .SetTokens(null)
                    .Build();
            };

            result.Should().ThrowExactly<AccountTokensNullException>()
                .WithMessage("Tokens argument is required.");
        }

        [Fact]
        public void Should_Throw_AccountTokensInvalidException_When_Tokens_Contains_Null()
        {
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .SetTokens(new List<Token> { null })
                    .Build();
            };

            result.Should().ThrowExactly<AccountTokensInvalidException>()
                .WithMessage("Tokens argument is invalid.");
        }

        [Fact]
        public void Should_Throw_AccountTokensDuplicateValuesException_When_Tokens_Contains_Duplicated_Values()
        {
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue("Value")
                .Build();
            Action result = () =>
            {
                var unused = Account.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetConfirmed(false)
                    .SetPasswordHash("PasswordHash")
                    .SetSecurityStamp(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetRoles(new List<Guid>())
                    .SetTokens(new List<Token> { token, token })
                    .Build();
            };

            result.Should().ThrowExactly<AccountTokensDuplicateValuesException>()
                .WithMessage("Tokens argument contains duplicate values.");
        }

        [Fact]
        public void AddCreatedEvent_Should_Add_AccountCreatedDomainEvent()
        {
            var correlationId = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            account.AddCreatedEvent(correlationId);

            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountCreatedDomainEvent));
            var accountCreatedDomainEvent = (AccountCreatedDomainEvent)account.DomainEvents.First();
            accountCreatedDomainEvent.AggregateId.Should().Be(account.Id);
            accountCreatedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountCreatedDomainEvent.Email.Should().Be(account.Email);
            accountCreatedDomainEvent.Confirmed.Should().Be(account.Confirmed);
            accountCreatedDomainEvent.Created.Should().Be(account.Created);
            accountCreatedDomainEvent.PasswordHash.Should().Be(account.PasswordHash);
            accountCreatedDomainEvent.SecurityStamp.Should().Be(account.SecurityStamp);
        }

        [Fact]
        public void AddCreatedEvent_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            Action result = () => account.AddCreatedEvent(new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void AddCreatedEvent_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Guid_Empty()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            Action result = () => account.AddCreatedEvent(Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void AddDeletedEvent_Should_Add_AccountDeletedDomainEvent()
        {
            var correlationId = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            account.AddDeletedEvent(correlationId);

            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountDeletedDomainEvent));
            var accountDeletedDomainEvent = (AccountDeletedDomainEvent)account.DomainEvents.First();
            accountDeletedDomainEvent.AggregateId.Should().Be(account.Id);
            accountDeletedDomainEvent.CorrelationId.Should().Be(correlationId);
        }

        [Fact]
        public void AddDeletedEvent_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            Action result = () => account.AddDeletedEvent(new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void AddDeletedEvent_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Guid_Empty()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            Action result = () => account.AddDeletedEvent(Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void GenerateToken_Should_Generate_Token_For_First_Time()
        {
            var correlationId = Guid.NewGuid();
            var tokenType = TokenTypeEnumeration.AccountConfirmation;
            var securityStamp = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(securityStamp)
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            var result = account.GenerateToken(tokenType, correlationId);

            result.Type.Should().BeEquivalentTo(tokenType);
            account.Tokens.Should().Contain(result);
            account.SecurityStamp.Should().NotBe(securityStamp);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountSecurityStampChangedDomainEvent));
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountTokenGeneratedDomainEvent));
            var accountSecurityStampChangedDomainEvent = (AccountSecurityStampChangedDomainEvent)account.DomainEvents.First();
            var accountTokenGeneratedDomainEvent = (AccountTokenGeneratedDomainEvent)account.DomainEvents.Last();
            accountSecurityStampChangedDomainEvent.AggregateId.Should().Be(account.Id);
            accountTokenGeneratedDomainEvent.AggregateId.Should().Be(account.Id);
            accountSecurityStampChangedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountTokenGeneratedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountTokenGeneratedDomainEvent.Token.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void GenerateToken_Should_Generate_New_Token_And_Remove_Old_One()
        {
            var correlationId = Guid.NewGuid();
            var tokenType = TokenTypeEnumeration.PasswordReset;
            var securityStamp = Guid.NewGuid();
            var oldToken = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(tokenType)
                .SetValue("OldValue")
                .Build();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(securityStamp)
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .SetTokens(new List<Token>{ oldToken })
                .Build();

            var result = account.GenerateToken(tokenType, correlationId);

            result.Type.Should().BeEquivalentTo(tokenType);
            account.Tokens.Should().Contain(result);
            account.Tokens.Should().NotContain(oldToken);
            account.SecurityStamp.Should().NotBe(securityStamp);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountSecurityStampChangedDomainEvent));
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountTokenGeneratedDomainEvent));
            var accountSecurityStampChangedDomainEvent = (AccountSecurityStampChangedDomainEvent)account.DomainEvents.First();
            var accountTokenGeneratedDomainEvent = (AccountTokenGeneratedDomainEvent)account.DomainEvents.Last();
            accountSecurityStampChangedDomainEvent.AggregateId.Should().Be(account.Id);
            accountTokenGeneratedDomainEvent.AggregateId.Should().Be(account.Id);
            accountSecurityStampChangedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountTokenGeneratedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountTokenGeneratedDomainEvent.Token.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void GenerateToken_Should_Throw_TokenTypeNullException_When_TokenType_Is_Null()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.GenerateToken(null, Guid.NewGuid());

            result.Should().ThrowExactly<TokenTypeNullException>()
                .WithMessage("TokenType argument is required.");
        }

        [Fact]
        public void GenerateToken_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.GenerateToken(TokenTypeEnumeration.AccountConfirmation, new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void GenerateToken_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Guid_Empty()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.GenerateToken(TokenTypeEnumeration.AccountConfirmation, Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void Confirm_Should_Confirm_Account()
        {
            var securityStamp = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue("OldValue")
                .Build();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(securityStamp)
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .SetTokens(new List<Token> { token })
                .Build();

            account.Confirm(correlationId);

            account.Confirmed.Should().BeTrue();
            account.SecurityStamp.Should().NotBe(securityStamp);
            account.Tokens.Should().NotContain(token);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountSecurityStampChangedDomainEvent));
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountConfirmedDomainEvent));
            var accountSecurityStampChangedDomainEvent = (AccountSecurityStampChangedDomainEvent)account.DomainEvents.First();
            var accountConfirmedDomainEvent = (AccountConfirmedDomainEvent)account.DomainEvents.Last();
            accountSecurityStampChangedDomainEvent.AggregateId.Should().Be(account.Id);
            accountConfirmedDomainEvent.AggregateId.Should().Be(account.Id);
            accountSecurityStampChangedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountConfirmedDomainEvent.CorrelationId.Should().Be(correlationId);
        }

        [Fact]
        public void Confirm_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            Action result = () => account.Confirm(new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void Confirm_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Guid_Empty()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            Action result = () => account.Confirm(Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void Login_Should_Change_LastLogin()
        {
            var lastLogin = DateTimeOffset.UtcNow;
            var correlationId = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .SetLastLogin(lastLogin)
                .Build();

            account.Login(correlationId);

            account.LastLogin.Should().NotBe(lastLogin);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountLoggedInDomainEvent));
            var accountLoggedInDomainEvent = (AccountLoggedInDomainEvent)account.DomainEvents.First();
            accountLoggedInDomainEvent.AggregateId.Should().Be(account.Id);
            accountLoggedInDomainEvent.CorrelationId.Should().Be(correlationId);
            accountLoggedInDomainEvent.LastLogin.Should().NotBe(lastLogin);
        }

        [Fact]
        public void Login_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.Login(new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void Login_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Guid_Empty()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.Login(Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void ChangePassword_Should_Change_Password()
        {
            var securityStamp = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(securityStamp)
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();
            const string newPasswordHash = "NewPasswordHash";
            var correlationId = Guid.NewGuid();

            account.ChangePassword(newPasswordHash, correlationId);

            account.PasswordHash.Should().Be(newPasswordHash);
            account.SecurityStamp.Should().NotBe(securityStamp);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountSecurityStampChangedDomainEvent));
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountPasswordChangedDomainEvent));
            var accountSecurityStampChangedDomainEvent = (AccountSecurityStampChangedDomainEvent)account.DomainEvents.First();
            var accountPasswordChangedDomainEvent = (AccountPasswordChangedDomainEvent)account.DomainEvents.Last();
            accountSecurityStampChangedDomainEvent.AggregateId.Should().Be(account.Id);
            accountPasswordChangedDomainEvent.AggregateId.Should().Be(account.Id);
            accountPasswordChangedDomainEvent.PasswordHash.Should().Be(newPasswordHash);
            accountSecurityStampChangedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountPasswordChangedDomainEvent.CorrelationId.Should().Be(correlationId);
        }

        [Fact]
        public void ChangePassword_Should_Change_Password_And_Remove_PasswordReset_Token()
        {
            var securityStamp = Guid.NewGuid();
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.PasswordReset)
                .SetValue("Value")
                .Build();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(securityStamp)
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .SetTokens(new List<Token>{ token })
                .Build();
            const string newPasswordHash = "NewPasswordHash";
            var correlationId = Guid.NewGuid();

            account.ChangePassword(newPasswordHash, correlationId);

            account.PasswordHash.Should().Be(newPasswordHash);
            account.SecurityStamp.Should().NotBe(securityStamp);
            account.Tokens.Should().NotContain(token);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountSecurityStampChangedDomainEvent));
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountPasswordChangedDomainEvent));
            var accountPasswordChangedDomainEvent = (AccountPasswordChangedDomainEvent)account.DomainEvents.Last();
            accountPasswordChangedDomainEvent.AggregateId.Should().Be(account.Id);
            accountPasswordChangedDomainEvent.PasswordHash.Should().Be(newPasswordHash);
            accountPasswordChangedDomainEvent.CorrelationId.Should().Be(correlationId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void ChangePassword_Should_Throw_AccountPasswordHashNullException_When_PasswordHash_Is_Null_Or_Empty(string passwordHash)
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.ChangePassword(passwordHash, Guid.NewGuid());

            result.Should().ThrowExactly<AccountPasswordHashNullException>()
                .WithMessage("PasswordHash argument is required.");
        }

        [Fact]
        public void AddRole_Should_Add_Role_When_Account_Does_Not_Contain_This_Role()
        {
            var correlationId = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();
            var role = Guid.NewGuid();

            account.AddRole(role, correlationId);

            account.Roles.Should().Contain(role);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountRoleAddedDomainEvent));
            var accountRoleAddedDomainEvent = (AccountRoleAddedDomainEvent)account.DomainEvents.First();
            accountRoleAddedDomainEvent.AggregateId.Should().Be(account.Id);
            accountRoleAddedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountRoleAddedDomainEvent.Role.Should().Be(role);
        }

        [Fact]
        public void AddRole_Should_Throw_AccountRoleNullException_When_Role_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.AddRole(new Guid(), Guid.NewGuid());

            result.Should().ThrowExactly<AccountRoleNullException>()
                .WithMessage("Role argument is required.");
        }

        [Fact]
        public void AddRole_Should_Throw_AccountRoleNullException_When_Role_Is_Empty_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.AddRole(Guid.Empty, Guid.NewGuid());

            result.Should().ThrowExactly<AccountRoleNullException>()
                .WithMessage("Role argument is required.");
        }

        [Fact]
        public void AddRole_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.AddRole(Guid.NewGuid(), new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void AddRole_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Empty_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>())
                .Build();

            Action result = () => account.AddRole(Guid.NewGuid(), Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void RemoveRole_Should_Remove_Role_Account_Contains_This_Role()
        {
            var role = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { role })
                .Build();

            account.RemoveRole(role, correlationId);

            account.Roles.Should().NotContain(role);
            account.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountRoleDeletedDomainEvent));
            var accountRoleDeletedDomainEvent = (AccountRoleDeletedDomainEvent)account.DomainEvents.First();
            accountRoleDeletedDomainEvent.AggregateId.Should().Be(account.Id);
            accountRoleDeletedDomainEvent.CorrelationId.Should().Be(correlationId);
            accountRoleDeletedDomainEvent.Role.Should().Be(role);
        }

        [Fact]
        public void RemoveRole_Should_Throw_AccountRoleNullException_When_Role_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => account.AddRole(new Guid(), Guid.NewGuid());

            result.Should().ThrowExactly<AccountRoleNullException>()
                .WithMessage("Role argument is required.");
        }

        [Fact]
        public void RemoveRole_Should_Throw_AccountRoleNullException_When_Role_Is_Empty_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => account.AddRole(Guid.Empty, Guid.NewGuid());

            result.Should().ThrowExactly<AccountRoleNullException>()
                .WithMessage("Role argument is required.");
        }

        [Fact]
        public void RemoveRole_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_New_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => account.AddRole(Guid.NewGuid(), new Guid());

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void RemoveRole_Should_Throw_AccountCorrelationIdNullException_When_CorrelationId_Is_Empty_Guid()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => account.AddRole(Guid.NewGuid(), Guid.Empty);

            result.Should().ThrowExactly<AccountCorrelationIdNullException>()
                .WithMessage("CorrelationId argument is required.");
        }

        [Fact]
        public void ApplyEvents_Should_Apply_Events_To_Revert_User_State()
        {
            var accountId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var lastLogin = DateTimeOffset.UtcNow;
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.PasswordReset)
                .SetValue("12345")
                .Build();
            const string passwordHash = "NewPasswordHash";
            var domainEvents = new List<IDomainEvent>
            {
                new AccountConfirmedDomainEvent(accountId, correlationId),
                new AccountLoggedInDomainEvent(accountId, correlationId, lastLogin),
                new AccountTokenGeneratedDomainEvent(accountId, correlationId, token),
                new AccountPasswordChangedDomainEvent(accountId, correlationId, passwordHash)
            };
            var account = Account.Builder()
                .SetId(accountId)
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();

            account.AddEvents(domainEvents);
            account.ApplyEvents();

            account.Confirmed.Should().BeTrue();
            account.LastLogin.Should().Be(lastLogin);
            account.Tokens.Should().BeEmpty();
            account.PasswordHash.Should().Be(passwordHash);
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
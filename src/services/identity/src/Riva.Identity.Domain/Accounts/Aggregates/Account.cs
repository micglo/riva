using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Identity.Domain.Accounts.Builders;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Events;
using Riva.Identity.Domain.Accounts.Services;
using Riva.Identity.Domain.Accounts.ValueObjects.EntityValueObjects.TokenValueObjects;

namespace Riva.Identity.Domain.Accounts.Aggregates
{
    public class Account : AggregateBase
    {
        private readonly List<Guid> _roles;
        private readonly List<Token> _tokens;
        private readonly TokenGeneratorService _tokenGeneratorService;

        public string Email { get; private set; }
        public bool Confirmed { get; private set; }
        public string PasswordHash { get; private set; }
        public Guid SecurityStamp { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset? LastLogin { get; private set; }
        public IReadOnlyCollection<Guid> Roles => _roles.AsReadOnly();
        public IReadOnlyCollection<Token> Tokens => _tokens.AsReadOnly();

        private Account(AccountBuilder builder) : base(builder.Id)
        {
            Email = builder.Email;
            Confirmed = builder.Confirmed;
            PasswordHash = builder.PasswordHash;
            SecurityStamp = builder.SecurityStamp;
            Created = builder.Created;
            LastLogin = builder.LastLogin;
            _roles = builder.Roles;
            _tokens = builder.Tokens;
            _tokenGeneratorService = new TokenGeneratorService();
        }

        public static IAccountIdSetter Builder()
        {
            return new AccountBuilder();
        }

        public void AddCreatedEvent(Guid correlationId)
        {
            correlationId = new AccountCorrelationId(correlationId);
            AddEvent(new AccountCreatedDomainEvent(Id, correlationId, Email, Confirmed, PasswordHash, SecurityStamp,
                Created, LastLogin));
        }

        public void AddDeletedEvent(Guid correlationId)
        {
            correlationId = new AccountCorrelationId(correlationId);
            AddEvent(new AccountDeletedDomainEvent(Id, correlationId));
        }

        public Token GenerateToken(TokenTypeEnumeration tokenType, Guid correlationId)
        {
            tokenType = new TokenType(tokenType);
            correlationId = new AccountCorrelationId(correlationId);
            var token = _tokens.SingleOrDefault(x => x.Type.Equals(tokenType));
            _tokens.Remove(token);
            ChangeSecurityStamp(correlationId);
            token = _tokenGeneratorService.Generate(Id, SecurityStamp, tokenType);
            _tokens.Add(token);
            AddEvent(new AccountTokenGeneratedDomainEvent(Id, correlationId, token));
            return token;
        }

        public void Confirm(Guid correlationId)
        {
            correlationId = new AccountCorrelationId(correlationId);
            Confirmed = true;
            ChangeSecurityStamp(correlationId);
            var token = _tokens.SingleOrDefault(x => x.Type.Equals(TokenTypeEnumeration.AccountConfirmation));
            _tokens.Remove(token);
            AddEvent(new AccountConfirmedDomainEvent(Id, correlationId));
        }

        public void Login(Guid correlationId)
        {
            correlationId = new AccountCorrelationId(correlationId);
            LastLogin = DateTimeOffset.UtcNow;
            AddEvent(new AccountLoggedInDomainEvent(Id, correlationId, LastLogin.Value));
        }

        public void ChangePassword(string passwordHash, Guid correlationId)
        {
            PasswordHash = new AccountPasswordHash(passwordHash);
            ChangeSecurityStamp(correlationId);
            var token = _tokens.SingleOrDefault(x => x.Type.Equals(TokenTypeEnumeration.PasswordReset));
            _tokens.Remove(token);
            AddEvent(new AccountPasswordChangedDomainEvent(Id, correlationId, PasswordHash));
        }

        public void AddRole(Guid role, Guid correlationId)
        {
            role = new AccountRole(role);
            correlationId = new AccountCorrelationId(correlationId);
            var anyDuplicates = _roles.Any(x => x == role);
            if (!anyDuplicates)
            {
                _roles.Add(role);
                AddEvent(new AccountRoleAddedDomainEvent(Id, correlationId, role));
            }
        }

        public void RemoveRole(Guid role, Guid correlationId)
        {
            role = new AccountRole(role);
            correlationId = new AccountCorrelationId(correlationId);

            if (_roles.Contains(role))
            {
                _roles.Remove(role);
                AddEvent(new AccountRoleDeletedDomainEvent(Id, correlationId, role));
            }
        }

        public override void ApplyEvents()
        {
            base.ApplyEvents();

            foreach (var domainEvent in Events)
                switch (domainEvent)
                {
                    case AccountCreatedDomainEvent accountCreatedDomainEvent:
                    {
                        Email = accountCreatedDomainEvent.Email;
                        Confirmed = accountCreatedDomainEvent.Confirmed;
                        PasswordHash = accountCreatedDomainEvent.PasswordHash;
                        SecurityStamp = accountCreatedDomainEvent.SecurityStamp;
                        Created = accountCreatedDomainEvent.Created;
                        LastLogin = accountCreatedDomainEvent.LastLogin;
                        break;
                    }
                    case AccountSecurityStampChangedDomainEvent accountSecurityStampChangedDomainEvent:
                    {
                        SecurityStamp = accountSecurityStampChangedDomainEvent.SecurityStamp;
                        break;
                    }
                    case AccountTokenGeneratedDomainEvent accountTokenGeneratedDomainEvent:
                    {
                        var token = _tokens.SingleOrDefault(x => x.Type.Equals(accountTokenGeneratedDomainEvent.Token.Type));
                        _tokens.Remove(token);
                        _tokens.Add(accountTokenGeneratedDomainEvent.Token);
                        break;
                    }
                    case AccountConfirmedDomainEvent _:
                    {
                        var token = _tokens.SingleOrDefault(x => x.Type.Equals(TokenTypeEnumeration.AccountConfirmation));
                        _tokens.Remove(token);
                        Confirmed = true;
                        break;
                    }
                    case AccountLoggedInDomainEvent accountLoggedInDomainEvent:
                    {
                        LastLogin = accountLoggedInDomainEvent.LastLogin;
                        break;
                    }
                    case AccountPasswordChangedDomainEvent accountPasswordChangedDomainEvent:
                    {
                        var token = _tokens.SingleOrDefault(x => x.Type.Equals(TokenTypeEnumeration.PasswordReset));
                        _tokens.Remove(token);
                        PasswordHash = accountPasswordChangedDomainEvent.PasswordHash;
                        break;
                    }
                    case AccountRoleAddedDomainEvent accountRoleAddedDomainEvent:
                    {
                        _roles.Add(accountRoleAddedDomainEvent.Role);
                        break;
                    }
                    case AccountRoleDeletedDomainEvent accountRoleDeletedDomainEvent:
                    {
                        _roles.Remove(accountRoleDeletedDomainEvent.Role);
                        break;
                    }
                }
        }

        private void ChangeSecurityStamp(Guid correlationId)
        {
            SecurityStamp = Guid.NewGuid();
            correlationId = new AccountCorrelationId(correlationId);
            AddEvent(new AccountSecurityStampChangedDomainEvent(Id, correlationId, SecurityStamp));
        }

        private class AccountBuilder : IAccountIdSetter, IAccountEmailSetter, IAccountConfirmedSetter, IAccountPasswordHashSetter,
            IAccountSecurityStampSetter, IAccountCreatedSetter, IAccountBuilder
        {
            public Guid Id { get; private set; }
            public string Email { get; private set; }
            public bool Confirmed { get; private set; }
            public string PasswordHash { get; private set; }
            public Guid SecurityStamp { get; private set; }
            public DateTimeOffset Created { get; private set; }
            public DateTimeOffset? LastLogin { get; private set; }
            public List<Guid> Roles { get; private set; } = new List<Guid>();
            public List<Token> Tokens { get; private set; } = new List<Token>();

            public IAccountEmailSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public IAccountConfirmedSetter SetEmail(string email)
            {
                Email = new AccountEmail(email);
                return this;
            }

            public IAccountPasswordHashSetter SetConfirmed(bool confirmed)
            {
                Confirmed = confirmed;
                return this;
            }

            public IAccountSecurityStampSetter SetPasswordHash(string passwordHash)
            {
                PasswordHash = passwordHash;
                return this;
            }

            public IAccountCreatedSetter SetSecurityStamp(Guid securityStamp)
            {
                SecurityStamp = new AccountSecurityStamp(securityStamp);
                return this;
            }

            public IAccountBuilder SetCreated(DateTimeOffset created)
            {
                Created = created;
                return this;
            }

            public IAccountBuilder SetLastLogin(DateTimeOffset? lastLogin)
            {
                LastLogin = lastLogin;
                return this;
            }

            public IAccountBuilder SetRoles(IEnumerable<Guid> roles)
            {
                Roles = new AccountRoles(roles);
                return this;
            }

            public IAccountBuilder SetTokens(IEnumerable<Token> tokens)
            {
                Tokens = new AccountTokens(tokens);
                return this;
            }

            public Account Build()
            {
                return new Account(this);
            }
        }
    }
}
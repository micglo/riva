using System;
using System.Collections.Generic;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;

namespace Riva.Identity.Domain.Accounts.Builders
{
    public interface IAccountIdSetter
    {
        IAccountEmailSetter SetId(Guid id);
    }

    public interface IAccountEmailSetter
    {
        IAccountConfirmedSetter SetEmail(string email);
    }

    public interface IAccountConfirmedSetter
    {
        IAccountPasswordHashSetter SetConfirmed(bool confirmed);
    }

    public interface IAccountPasswordHashSetter
    {
        IAccountSecurityStampSetter SetPasswordHash(string passwordHash);
    }

    public interface IAccountSecurityStampSetter
    {
        IAccountCreatedSetter SetSecurityStamp(Guid securityStamp);
    }

    public interface IAccountCreatedSetter
    {
        IAccountBuilder SetCreated(DateTimeOffset created);
    }

    public interface IAccountBuilder
    {
        IAccountBuilder SetLastLogin(DateTimeOffset? lastLogin);
        IAccountBuilder SetRoles(IEnumerable<Guid> roles);
        IAccountBuilder SetTokens(IEnumerable<Token> tokens);
        Account Build();
    }
}
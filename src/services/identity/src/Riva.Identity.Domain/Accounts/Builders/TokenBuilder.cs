using System;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;

namespace Riva.Identity.Domain.Accounts.Builders
{
    public interface ITokenIssuedSetter
    {
        ITokenExpiresSetter SetIssued(DateTimeOffset issued);
    }

    public interface ITokenExpiresSetter
    {
        ITokenTypeSetter SetExpires(DateTimeOffset expires);
    }

    public interface ITokenTypeSetter
    {
        ITokenValueSetter SetType(TokenTypeEnumeration type);
    }

    public interface ITokenValueSetter
    {
        ITokenBuilder SetValue(string value);
    }

    public interface ITokenBuilder
    {
        Token Build();
    }
}
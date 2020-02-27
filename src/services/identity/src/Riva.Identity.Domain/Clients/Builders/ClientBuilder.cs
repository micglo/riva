using System;
using System.Collections.Generic;
using Riva.Identity.Domain.Clients.Aggregates;

namespace Riva.Identity.Domain.Clients.Builders
{
    public interface IClientIdSetter
    {
        IClientEnabledSetter SetId(Guid id);
    }

    public interface IClientEnabledSetter
    {
        IClientEnableLocalLoginSetter SetEnabled(bool enabled);
    }

    public interface IClientEnableLocalLoginSetter
    {
        IClientRequirePkceSetter SetEnableLocalLogin(bool enableLocalLogin);
    }

    public interface IClientRequirePkceSetter
    {
        IClientBuilder SetRequirePkce(bool requirePkce);
    }

    public interface IClientBuilder
    {
        IClientBuilder SetIdentityProviderRestrictions(IEnumerable<string> identityProviderRestrictions);
        Client Build();
    }
}
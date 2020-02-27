using System;
using System.Collections.Generic;
using Riva.Identity.Domain.Clients.Builders;
using Riva.Identity.Domain.Clients.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Clients.Aggregates
{
    public class Client : AggregateBase
    {
        public bool Enabled { get; }
        public bool EnableLocalLogin { get; }
        public bool RequirePkce { get; }
        public IReadOnlyCollection<string> IdentityProviderRestrictions { get; }

        private Client(ClientBuilder builder) : base(builder.Id)
        {
            Enabled = builder.Enabled;
            EnableLocalLogin = builder.EnableLocalLogin;
            RequirePkce = builder.RequirePkce;
            IdentityProviderRestrictions = builder.IdentityProviderRestrictions;
        }

        public static IClientIdSetter Builder()
        {
            return new ClientBuilder();
        }

        private class ClientBuilder : IClientIdSetter, IClientEnabledSetter, IClientEnableLocalLoginSetter, IClientRequirePkceSetter,
            IClientBuilder
        {
            public Guid Id { get; private set; }
            public bool Enabled { get; private set; }
            public bool EnableLocalLogin { get; private set; }
            public bool RequirePkce { get; private set; }
            public List<string> IdentityProviderRestrictions { get; private set; } = new List<string>();

            public IClientEnabledSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public IClientEnableLocalLoginSetter SetEnabled(bool enabled)
            {
                Enabled = enabled;
                return this;
            }

            public IClientRequirePkceSetter SetEnableLocalLogin(bool enableLocalLogin)
            {
                EnableLocalLogin = enableLocalLogin;
                return this;
            }

            public IClientBuilder SetRequirePkce(bool requirePkce)
            {
                RequirePkce = requirePkce;
                return this;
            }

            public IClientBuilder SetIdentityProviderRestrictions(IEnumerable<string> identityProviderRestrictions)
            {
                IdentityProviderRestrictions = new ClientIdentityProviderRestrictions(identityProviderRestrictions);
                return this;
            }

            public Client Build()
            {
                return new Client(this);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.Identity.Domain.Clients.Aggregates;
using Riva.Identity.Domain.Clients.Exceptions;
using Xunit;

namespace Riva.Identity.Domain.Test
{
    public class ClientTest
    {
        [Fact]
        public void Should_Create_Client()
        {
            var id = Guid.NewGuid();
            const bool enabled = true;
            const bool enableLocalLogin = true;
            const bool requirePkce = true;
            var identityProviderRestrictions = new List<string> { "Google" };

            var result = Client.Builder()
                .SetId(id)
                .SetEnabled(enabled)
                .SetEnableLocalLogin(enableLocalLogin)
                .SetRequirePkce(requirePkce)
                .SetIdentityProviderRestrictions(identityProviderRestrictions)
                .Build();

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Enabled.Should().Be(enabled);
            result.EnableLocalLogin.Should().Be(enableLocalLogin);
            result.RequirePkce.Should().Be(requirePkce);
            result.IdentityProviderRestrictions.Should().BeEquivalentTo(identityProviderRestrictions);
        }

        [Fact]
        public void Should_Throw_ClientIdentityProviderRestrictionsNullException_When_IdentityProviderRestrictions_Is_Null()
        {
            Action result = () =>
            {
                var unused = Client.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEnabled(true)
                    .SetEnableLocalLogin(true)
                    .SetRequirePkce(true)
                    .SetIdentityProviderRestrictions(null)
                    .Build();
            };

            result.Should().ThrowExactly<ClientIdentityProviderRestrictionsNullException>()
                .WithMessage("IdentityProviderRestrictions argument is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("")]
        public void Should_Throw_ClientIdentityProviderRestrictionsInvalidValuesException_When_IdentityProviderRestrictions_Contains_Empty_String(string value)
        {
            Action result = () =>
            {
                var unused = Client.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEnabled(true)
                    .SetEnableLocalLogin(true)
                    .SetRequirePkce(true)
                    .SetIdentityProviderRestrictions(new List<string>{ value })
                    .Build();
            };

            result.Should().ThrowExactly<ClientIdentityProviderRestrictionsInvalidValuesException>()
                .WithMessage("IdentityProviderRestrictions argument is invalid.");
        }
    }
}
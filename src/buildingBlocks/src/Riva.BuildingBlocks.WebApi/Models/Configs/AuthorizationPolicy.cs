using System;

namespace Riva.BuildingBlocks.WebApi.Models.Configs
{
    public class AuthorizationPolicy
    {
        public string Name { get; }
        public Microsoft.AspNetCore.Authorization.AuthorizationPolicy Policy { get; }

        public AuthorizationPolicy(string name, Microsoft.AspNetCore.Authorization.AuthorizationPolicy policy)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }
    }
}
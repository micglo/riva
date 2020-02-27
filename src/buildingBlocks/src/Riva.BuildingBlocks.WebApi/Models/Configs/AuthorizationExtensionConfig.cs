using System;
using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.WebApi.Models.Configs
{
    public class AuthorizationExtensionConfig
    {
        public string ApiResourceName { get; }
        public IEnumerable<AuthorizationPolicy> Policies { get; }

        public AuthorizationExtensionConfig(string apiResourceName, params AuthorizationPolicy[] policies)
        {
            ApiResourceName = !string.IsNullOrWhiteSpace(apiResourceName) ? apiResourceName : throw new ArgumentNullException(nameof(apiResourceName));
            Policies = policies != null && policies.Any() ? policies.ToList() : new List<AuthorizationPolicy>();
        }
    }
}
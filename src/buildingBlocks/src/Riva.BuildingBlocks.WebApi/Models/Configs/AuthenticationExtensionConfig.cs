using System;
using Microsoft.AspNetCore.Hosting;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;

namespace Riva.BuildingBlocks.WebApi.Models.Configs
{
    public class AuthenticationExtensionConfig
    {
        public IWebHostEnvironment Environment { get; }
        public string Audience { get; }
        public string Authority { get; }
        public string DefaultScheme { get; }
        public string KeyVaultName { get; }
        public string SigningCredentialCertificateName { get; }

        public AuthenticationExtensionConfig(IWebHostEnvironment environment, string audience, string authority, string defaultScheme, 
            string keyVaultName, string signingCredentialCertificateName)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));

            if (environment.IsNotLocalOrDocker())
            {
                if (string.IsNullOrWhiteSpace(keyVaultName))
                    throw new ArgumentNullException(nameof(keyVaultName));
                if (string.IsNullOrWhiteSpace(signingCredentialCertificateName))
                    throw new ArgumentNullException(nameof(signingCredentialCertificateName));
            }

            Audience = !string.IsNullOrWhiteSpace(audience) ? audience : throw new ArgumentNullException(nameof(audience));
            Authority = !string.IsNullOrWhiteSpace(authority) ? authority : throw new ArgumentNullException(nameof(authority));
            DefaultScheme = defaultScheme;
            KeyVaultName = keyVaultName;
            SigningCredentialCertificateName = signingCredentialCertificateName;
        }
    }
}
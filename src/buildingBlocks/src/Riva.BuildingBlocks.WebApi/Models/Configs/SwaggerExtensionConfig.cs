using System;

namespace Riva.BuildingBlocks.WebApi.Models.Configs
{
    public class SwaggerExtensionConfig
    {
        public string ApiResourceName { get; }
        public string IdentityUrl { get; }

        public SwaggerExtensionConfig(string apiResourceName, string identityUrl)
        {
            ApiResourceName = !string.IsNullOrWhiteSpace(apiResourceName) ? apiResourceName : throw new ArgumentNullException(nameof(apiResourceName));
            IdentityUrl = !string.IsNullOrWhiteSpace(identityUrl) ? identityUrl : throw new ArgumentNullException(nameof(identityUrl));
        }
    }
}
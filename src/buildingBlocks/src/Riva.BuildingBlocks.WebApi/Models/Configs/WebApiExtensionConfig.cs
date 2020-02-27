using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

namespace Riva.BuildingBlocks.WebApi.Models.Configs
{
    public class WebApiExtensionConfig
    {
        private readonly IEnumerable<Assembly> _autoMapperAssemblies;
        public IWebHostEnvironment Environment { get; }
        public Assembly StartupAssembly { get; }
        public AuthorizationExtensionConfig AuthorizationExtensionConfig { get; }
        public AuthenticationExtensionConfig AuthenticationExtensionConfig { get; }
        public SwaggerExtensionConfig SwaggerExtensionConfig { get; }
        public IReadOnlyCollection<Assembly> AutoMapperAssemblies => _autoMapperAssemblies.ToList().AsReadOnly();

        public WebApiExtensionConfig(
            IWebHostEnvironment environment,
            Assembly startupAssembly,
            AuthorizationExtensionConfig authorizationExtensionConfig,
            AuthenticationExtensionConfig authenticationExtensionConfig,
            SwaggerExtensionConfig swaggerExtensionConfig,
            params Assembly[] autoMapperAssemblies)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            StartupAssembly = startupAssembly ?? throw new ArgumentNullException(nameof(startupAssembly));
            AuthorizationExtensionConfig = authorizationExtensionConfig ?? throw new ArgumentNullException(nameof(authorizationExtensionConfig));
            AuthenticationExtensionConfig = authenticationExtensionConfig ?? throw new ArgumentNullException(nameof(authenticationExtensionConfig));
            SwaggerExtensionConfig = swaggerExtensionConfig ?? throw new ArgumentNullException(nameof(swaggerExtensionConfig));
            _autoMapperAssemblies = autoMapperAssemblies is null || autoMapperAssemblies.Length == 0 ? new []{ startupAssembly } : autoMapperAssemblies;
        }
    }
}
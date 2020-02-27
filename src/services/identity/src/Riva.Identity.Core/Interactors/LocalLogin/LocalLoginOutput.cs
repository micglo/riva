using System.Collections.Generic;
using System.Linq;

namespace Riva.Identity.Core.Interactors.LocalLogin
{
    public class LocalLoginOutput
    {
        public bool LocalLoginEnabled { get; }
        public bool IsExternalLoginOnly { get; }
        public string ExternalLoginScheme { get; }
        public IReadOnlyCollection<LocalLoginExternalProviderOutput> ExternalProviders { get; }

        public LocalLoginOutput(bool localLoginEnabled, IEnumerable<LocalLoginExternalProviderOutput> externalProviders)
        {
            LocalLoginEnabled = localLoginEnabled;
            ExternalProviders = externalProviders.ToList().AsReadOnly();
            IsExternalLoginOnly = LocalLoginEnabled == false && ExternalProviders?.Count == 1;
            ExternalLoginScheme = IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
        }
    }
}
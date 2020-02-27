namespace Riva.Identity.Core.Interactors.LocalLogin
{
    public class LocalLoginExternalProviderOutput
    {
        public string DisplayName { get; }
        public string AuthenticationScheme { get; }

        public LocalLoginExternalProviderOutput(string displayName, string authenticationScheme)
        {
            DisplayName = displayName;
            AuthenticationScheme = authenticationScheme;
        }
    }
}
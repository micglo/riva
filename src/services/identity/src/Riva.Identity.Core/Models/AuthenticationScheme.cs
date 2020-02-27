namespace Riva.Identity.Core.Models
{
    public class AuthenticationScheme
    {
        public string Name { get; }
        public string DisplayName { get; }

        public AuthenticationScheme(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
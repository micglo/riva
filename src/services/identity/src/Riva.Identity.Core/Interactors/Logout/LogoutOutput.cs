namespace Riva.Identity.Core.Interactors.Logout
{
    public class LogoutOutput
    {
        public string LogoutId { get; }
        public bool ShowLogoutPrompt { get; }

        public LogoutOutput(string logoutId, bool showLogoutPrompt)
        {
            LogoutId = logoutId;
            ShowLogoutPrompt = showLogoutPrompt;
        }
    }
}
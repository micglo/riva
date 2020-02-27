namespace Riva.Identity.Web.Models.ViewModels
{
    public class LogoutViewModel
    {
        public string LogoutId { get; }
        public bool ShowLogoutPrompt { get; }

        public LogoutViewModel(string logoutId, bool showLogoutPrompt)
        {
            LogoutId = logoutId;
            ShowLogoutPrompt = showLogoutPrompt;
        }
    }
}
namespace Riva.Identity.Web.Models.ViewModels
{
    public class RedirectViewModel
    {
        public string RedirectUrl { get; }

        public RedirectViewModel(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }
    }
}
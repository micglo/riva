using System.Collections.Generic;
using System.Linq;
using Riva.Identity.Web.Models.Requests;

namespace Riva.Identity.Web.Models.ViewModels
{
    public class LocalLoginViewModel : LocalLoginRequest
    {
        private List<string> _errors;

        public bool LocalLoginEnabled { get; }
        public bool GoogleLoginEnabled { get; }
        public bool FacebookLoginEnabled { get; }
        public bool HasError { get; private set; }
        public string RivaWebRegistrationUrl { get; }
        public string RivaWebRequestRegistrationConfirmationEmailUrl { get; }
        public string RivaWebRequestPasswordResetEmailUrl { get; }
        public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

        public LocalLoginViewModel(bool localLoginEnabled, bool googleLoginEnabled, bool facebookLoginEnabled, 
            string rivaWebRegistrationUrl, string rivaWebRequestRegistrationConfirmationEmailUrl, 
            string rivaWebRequestPasswordResetEmailUrl)
        {
            LocalLoginEnabled = localLoginEnabled;
            GoogleLoginEnabled = googleLoginEnabled;
            FacebookLoginEnabled = facebookLoginEnabled;
            RivaWebRegistrationUrl = rivaWebRegistrationUrl;
            RivaWebRequestRegistrationConfirmationEmailUrl = rivaWebRequestRegistrationConfirmationEmailUrl;
            RivaWebRequestPasswordResetEmailUrl = rivaWebRequestPasswordResetEmailUrl;
            _errors = new List<string>();
        }

        public void SetErrors(IEnumerable<string> errors)
        {
            HasError = true;
            _errors = errors.ToList();
        }
    }
}
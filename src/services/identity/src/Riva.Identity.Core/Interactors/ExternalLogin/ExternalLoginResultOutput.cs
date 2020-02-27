namespace Riva.Identity.Core.Interactors.ExternalLogin
{
    public class ExternalLoginResultOutput
    {
        public string ReturnUrl { get; }
        public bool? IsNativeClient { get; }

        public ExternalLoginResultOutput(string returnUrl, bool? isNativeClient)
        {
            ReturnUrl = returnUrl;
            IsNativeClient = isNativeClient;
        }
    }
}
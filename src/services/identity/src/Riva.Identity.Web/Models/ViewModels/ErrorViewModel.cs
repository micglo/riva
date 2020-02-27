namespace Riva.Identity.Web.Models.ViewModels
{
    public class ErrorViewModel
    {
        public int StatusCode { get; }
        public string Message { get; }

        public ErrorViewModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}

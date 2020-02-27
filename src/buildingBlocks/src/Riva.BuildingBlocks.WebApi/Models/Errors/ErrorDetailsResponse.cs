namespace Riva.BuildingBlocks.WebApi.Models.Errors
{
    public class ErrorDetailsResponse
    {
        public string ErrorCode { get; }
        public string ErrorMessage { get; }

        public ErrorDetailsResponse(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
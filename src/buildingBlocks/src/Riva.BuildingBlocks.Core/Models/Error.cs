namespace Riva.BuildingBlocks.Core.Models
{
    public class Error : IError
    {
        public IErrorCode ErrorCode { get; }
        public string ErrorMessage { get; }

        public Error(IErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Riva.BuildingBlocks.WebApi.Models.Errors
{
    public class ErrorResponse
    {
        public string Type { get; }
        public string Instance { get; }
        public int StatusCode { get; }
        public string ErrorCode { get; }
        public string ErrorMessage { get; }
        public IReadOnlyCollection<ErrorDetailsResponse> Details { get; }

        public ErrorResponse(string type, string instance, HttpStatusCode statusCode, string errorCode, string errorMessage)
        {
            Type = type;
            Instance = instance;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            StatusCode = (int)statusCode;
            Details = new List<ErrorDetailsResponse>().AsReadOnly();
        }

        public ErrorResponse(string type, string instance, HttpStatusCode statusCode, string errorCode, string errorMessage, IEnumerable<ErrorDetailsResponse> details)
        {
            Type = type;
            Instance = instance;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            StatusCode = (int)statusCode;
            Details = details.ToList().AsReadOnly();
        }
    }
}
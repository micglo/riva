using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes;
using Riva.BuildingBlocks.WebApi.Models.Errors;

namespace Riva.BuildingBlocks.WebApi.Controllers
{
    public abstract class RivaControllerBase : ControllerBase
    {
        protected IActionResult CreateErrorResult(HttpStatusCode httpStatusCode)
        {
            return CreateErrorResult(httpStatusCode, new List<ErrorDetailsResponse>());
        }

        protected IActionResult CreateErrorResult(HttpStatusCode httpStatusCode, IEnumerable<ErrorDetailsResponse> details)
        {
            var instance = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (feature != null)
            {
                var originalRequestUrl = $"{feature.OriginalPathBase}{feature.OriginalPath}{feature.OriginalQueryString}";
                instance = $"{Request.Scheme}://{Request.Host}{originalRequestUrl}";
            }

            var errorResponse = CreateErrorResponse(httpStatusCode, instance, details);
            return StatusCode((int)httpStatusCode, errorResponse);
        }

        protected void ValidatePathIdWithRequestBodyId(Guid pathId, Guid requestBodyId)
        {
            if (pathId != requestBodyId)
            {
                var errors = new Collection<IError>
                {
                    new Error(RequestErrorCodeEnumeration.IdMismatch, $"Request path id: {pathId} does not match request body id: {requestBodyId}.")
                };
                throw new ValidationException(errors);
            }
        }

        protected static ErrorResponse CreateErrorResponse(HttpStatusCode httpStatusCode, string instance)
        {
            return CreateErrorResponse(httpStatusCode, instance, new List<ErrorDetailsResponse>());
        }

        protected static ErrorResponse CreateErrorResponse(HttpStatusCode httpStatusCode, string instance, IEnumerable<ErrorDetailsResponse> details)
        {
            var errorMessage = GetErrorMessage(httpStatusCode);
            return CreateErrorResponse(httpStatusCode, instance, errorMessage, details);
        }

        protected static ErrorResponse CreateErrorResponse(HttpStatusCode httpStatusCode, string instance, string errorMessage, IEnumerable<ErrorDetailsResponse> details)
        {
            var type = GetErrorType(httpStatusCode);
            var errorCode = GetErrorCode(httpStatusCode);
            return new ErrorResponse(type, instance, httpStatusCode, errorCode.DisplayName, errorMessage, details);
        }

        private static string GetErrorType(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode switch
            {
                HttpStatusCode.BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                HttpStatusCode.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
                HttpStatusCode.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                HttpStatusCode.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                HttpStatusCode.MethodNotAllowed => "https://tools.ietf.org/html/rfc7231#section-6.5.5",
                HttpStatusCode.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                HttpStatusCode.PreconditionFailed => "https://tools.ietf.org/html/rfc7232#section-4.2",
                HttpStatusCode.UnsupportedMediaType => "https://tools.ietf.org/html/rfc7231#section-6.5.13",
                HttpStatusCode.UnprocessableEntity => "https://tools.ietf.org/html/rfc4918#section-11.2",
                HttpStatusCode.PreconditionRequired => "https://tools.ietf.org/html/rfc6585#section-3",
                HttpStatusCode.InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                _ => throw new ArgumentException($"{httpStatusCode} is not currently supported.")
            };
        }

        private static RequestErrorCodeEnumeration GetErrorCode(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode switch
            {
                HttpStatusCode.BadRequest => RequestErrorCodeEnumeration.BadRequest,
                HttpStatusCode.Unauthorized => RequestErrorCodeEnumeration.AuthorizationRequired,
                HttpStatusCode.Forbidden => RequestErrorCodeEnumeration.AccessDenied,
                HttpStatusCode.NotFound => RequestErrorCodeEnumeration.ResourceNotFound,
                HttpStatusCode.MethodNotAllowed => RequestErrorCodeEnumeration.MethodNotAllowed,
                HttpStatusCode.Conflict => RequestErrorCodeEnumeration.ConflictResource,
                HttpStatusCode.PreconditionFailed => RequestErrorCodeEnumeration.ConditionNotMet,
                HttpStatusCode.UnsupportedMediaType => RequestErrorCodeEnumeration.MediaTypeNotSupported,
                HttpStatusCode.UnprocessableEntity => RequestErrorCodeEnumeration.ValidationFailed,
                HttpStatusCode.PreconditionRequired => RequestErrorCodeEnumeration.PreconditionRequired,
                HttpStatusCode.InternalServerError => RequestErrorCodeEnumeration.ServerError,
                _ => throw new ArgumentException($"{httpStatusCode} is not currently supported.")
            };
        }

        private static string GetErrorMessage(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode switch
            {
                HttpStatusCode.BadRequest => "Bad request.",
                HttpStatusCode.Unauthorized => "Authorization is required.",
                HttpStatusCode.Forbidden => "Operation is forbidden.",
                HttpStatusCode.NotFound => "Resource not found.",
                HttpStatusCode.MethodNotAllowed => "Method is not allowed.",
                HttpStatusCode.Conflict => "The request could not be completed due to a conflict with the current state of the resource.",
                HttpStatusCode.PreconditionFailed => "The condition set in If-Match HTTP request header was not met.",
                HttpStatusCode.UnsupportedMediaType => "Media type is not supported",
                HttpStatusCode.UnprocessableEntity => "Validation failed.",
                HttpStatusCode.PreconditionRequired => "If-Match header is required.",
                HttpStatusCode.InternalServerError => "Unexpected error has occured.",
                _ => throw new ArgumentException($"{httpStatusCode} is not currently supported.")
            };
        }
    }
}
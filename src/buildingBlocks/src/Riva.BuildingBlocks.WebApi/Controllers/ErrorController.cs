using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApi.Attributes.Routes;
using Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes;
using Riva.BuildingBlocks.WebApi.Models.Errors;

namespace Riva.BuildingBlocks.WebApi.Controllers
{
    [ApiController]
    [ApiRoute("error")]
    public class ErrorController : RivaControllerBase
    {
        protected readonly ILogger Logger;

        public ErrorController(ILogger logger)
        {
            Logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorResponse = CreateErrorResponse();
            return StatusCode(errorResponse.StatusCode, errorResponse);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("{statusCode}")]
        public IActionResult StatusCodeError(int statusCode)
        {
            return CreateErrorResult((HttpStatusCode) statusCode);
        }

        protected ErrorResponse CreateErrorResponse()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var instance = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{feature.Path}";

            switch (feature.Error)
            {
                case ForbiddenException forbiddenException:
                {
                    var details =
                        forbiddenException.Errors.Select(x => new ErrorDetailsResponse(x.ErrorCode.DisplayName, x.ErrorMessage));
                    return CreateErrorResponse(HttpStatusCode.Forbidden, instance, details);
                }
                case ResourceNotFoundException resourceNotFoundException:
                {
                    var details =
                        resourceNotFoundException.Errors.Select(x => new ErrorDetailsResponse(x.ErrorCode.DisplayName, x.ErrorMessage));
                    return CreateErrorResponse(HttpStatusCode.NotFound, instance, details);
                }
                case ConflictException conflictException:
                {
                    var details = conflictException.Errors.Select(x =>
                        new ErrorDetailsResponse(x.ErrorCode.DisplayName, x.ErrorMessage));
                    return CreateErrorResponse(HttpStatusCode.Conflict, instance, details);
                }
                case PreconditionFailedException preconditionFailedException:
                {
                    var details = preconditionFailedException.Errors.Select(x =>
                        new ErrorDetailsResponse(x.ErrorCode.DisplayName, x.ErrorMessage));
                        return CreateErrorResponse(HttpStatusCode.PreconditionFailed, instance, details);
                }
                case ValidationException validationException:
                {
                    var details =
                        validationException.Errors.Select(x => new ErrorDetailsResponse(x.ErrorCode.DisplayName, x.ErrorMessage));
                    return CreateErrorResponse(HttpStatusCode.UnprocessableEntity, instance, details);
                }
                case DomainException domainException:
                {
                    var details = new List<ErrorDetailsResponse>
                    {
                        new ErrorDetailsResponse(RequestBodyValidationErrorCodeEnumeration.InvalidValue.DisplayName, domainException.Message)
                    };
                    return CreateErrorResponse(HttpStatusCode.UnprocessableEntity, instance, details);
                }
                case UnprocessableException unprocessableException:
                {
                    var details =
                        unprocessableException.Errors.Select(x => new ErrorDetailsResponse(x.ErrorCode.DisplayName, x.ErrorMessage));
                    var response = CreateErrorResponse(HttpStatusCode.InternalServerError, instance, feature.Error.Message, details);
                    Logger.LogError(
                        "Instance={instance}, Message={errorMessage}, Source={source}, StackTrace={stackTrace}",
                        response.Instance, feature.Error.Message, feature.Error.Source, feature.Error.StackTrace);
                    return response;
                }
                default:
                {
                    var response = CreateErrorResponse(HttpStatusCode.InternalServerError, instance);
                    Logger.LogError(
                        "Instance={instance}, Message={errorMessage}, Source={source}, StackTrace={stackTrace}",
                        response.Instance, feature.Error.Message, feature.Error.Source, feature.Error.StackTrace);
                    return response;
                }
            }
        }
    }
}
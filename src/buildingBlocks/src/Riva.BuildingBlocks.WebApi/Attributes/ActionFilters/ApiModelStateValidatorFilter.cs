using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes;
using Riva.BuildingBlocks.WebApi.Models.Errors;

namespace Riva.BuildingBlocks.WebApi.Attributes.ActionFilters
{
    public class ApiModelStateValidatorFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid || !context.HttpContext.Request.Path.Value.Contains("api"))
                return;
            
            const string type = "https://tools.ietf.org/html/rfc4918#section-11.2";
            var instance = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.PathBase}{context.HttpContext.Request.Path}";
            var details = GetErrorDetails(context.ModelState);
            var errorResponse = new ErrorResponse(type, instance, HttpStatusCode.UnprocessableEntity,
                RequestErrorCodeEnumeration.ValidationFailed.DisplayName, "Validation failed.", details);

            context.Result = new UnprocessableEntityObjectResult(errorResponse);
        }

        private static IEnumerable<ErrorDetailsResponse> GetErrorDetails(ModelStateDictionary state)
        {
            foreach (var key in state.Keys)
            {
                var modelError = state[key];
                foreach (var error in modelError.Errors)
                {
                    if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                        yield return new ErrorDetailsResponse(RequestBodyValidationErrorCodeEnumeration.InvalidValue.DisplayName,
                            error.ErrorMessage);
                    else if(error.Exception != null && !string.IsNullOrWhiteSpace(error.Exception.Message))
                        yield return new ErrorDetailsResponse(RequestBodyValidationErrorCodeEnumeration.InvalidValue.DisplayName,
                            error.Exception.Message);
                    else
                        yield return new ErrorDetailsResponse(RequestBodyValidationErrorCodeEnumeration.InvalidValue.DisplayName,
                            "Invalid value.");
                }
            }
        }
    }
}
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes;
using Riva.BuildingBlocks.WebApi.Models.Errors;

namespace Riva.BuildingBlocks.WebApi.Attributes.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireIfMatchAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            const string type = "https://tools.ietf.org/html/rfc6585#section-3";
            var instance = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.PathBase}{context.HttpContext.Request.Path}";
            var errorResponse = new ErrorResponse(type, instance, HttpStatusCode.PreconditionRequired,
                RequestErrorCodeEnumeration.PreconditionRequired.DisplayName, "If-Match header is required.");

            if (context.HttpContext.Request.Headers.ContainsKey(HeaderNames.IfMatch))
            {
                context.HttpContext.Request.Headers.TryGetValue(HeaderNames.IfMatch, out var value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.Result = new ObjectResult(errorResponse)
                    {
                        StatusCode = (int)HttpStatusCode.PreconditionRequired
                    };
                }
            }
            else
            {
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.PreconditionRequired
                };
            }
        }
    }
}
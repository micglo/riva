using System;
using Microsoft.AspNetCore.Mvc;

namespace Riva.BuildingBlocks.WebApi.Attributes.Routes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiRouteAttribute : RouteAttribute
    {
        public ApiRouteAttribute(string resourceName) : base($"api/{resourceName}")
        {
        }
    }
}
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Riva.BuildingBlocks.WebApi.Conventions
{
    public class ApiExplorerGroupPerRouteConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var attribute = action.Controller.Attributes.SingleOrDefault(x => x.GetType() == typeof(RouteAttribute));
            if (attribute != null)
            {
                var routeAttribute = (RouteAttribute)attribute;
                var index = routeAttribute.Template.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase);
                var groupName = routeAttribute.Template.Substring(index + 1);
                action.ApiExplorer.GroupName = groupName;
                action.Controller.ApiExplorer.GroupName = groupName;
                action.ApiExplorer.IsVisible = true;
                action.Controller.ApiExplorer.IsVisible = true;
            }
        }
    }
}
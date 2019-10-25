using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static partial class UrlHelperExtensions
    {
        private static readonly string ProviderIdKey = "ProviderId";
        public static string ProviderAction(this IUrlHelper url, string actionName)
        {
            if (url.ActionContext.RouteData.Values.ContainsKey(ProviderIdKey) && url.ActionContext.ActionDescriptor is ControllerActionDescriptor)
            {
                var controllerActionDescriptor = ((ControllerActionDescriptor)url.ActionContext.ActionDescriptor);
                var controllerName = controllerActionDescriptor.ControllerName;
                var providerId = url.ActionContext.RouteData.Values[ProviderIdKey].ToString();
                return url.Action(actionName, controllerName, new { ProviderId = providerId });
            }

            return url.Action(actionName);
        }
    }
}
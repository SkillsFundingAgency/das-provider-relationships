using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Filters
{
    public class ConfigurationViewBagFilter : ActionFilterAttribute
    {
        private readonly Func<ProviderRelationshipsConfiguration> _configuration;

        public ConfigurationViewBagFilter(Func<ProviderRelationshipsConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.AllowCollaborationPermission = _configuration().AllowCollaborationPermission;
        }
    }
}
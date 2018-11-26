using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SFA.DAS.ProviderRelationships.Api.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRemoteOnlyAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return actionContext.Request.RequestUri.IsLoopback || base.IsAuthorized(actionContext);
        }
    }
}
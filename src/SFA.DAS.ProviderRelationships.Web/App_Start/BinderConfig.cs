using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class BinderConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            binders.UseAuthorizationModelBinder();
        }
    }
}
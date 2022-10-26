using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Services;

public interface IAzureIdentityService
{
    Task<string> AuthenticationCallback(string authority, string resource, string scope);
}
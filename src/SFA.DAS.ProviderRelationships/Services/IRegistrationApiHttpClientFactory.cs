using SFA.DAS.Http;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRegistrationApiHttpClientFactory
    {
        IRestHttpClient CreateRestHttpClient();
    }
}
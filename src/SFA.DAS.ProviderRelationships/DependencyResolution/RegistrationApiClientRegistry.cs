using System.Net.Http;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.Http;
using SFA.DAS.NLog.Logger.Web.MessageHandlers;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class RegistrationApiClientRegistry : Registry
    {
        public RegistrationApiClientRegistry()
        {
            For<RegistrationApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().RegistrationApiClientConfiguration);
            For<IRegistrationApiConfiguration>().Use(c => c.GetInstance<RegistrationApiConfiguration>());
            For<IRegistrationApiClient>().Use<RegistrationApiClient>()
                .Ctor<HttpClient>().Is(c => CreateClient(c));
        }

        private HttpClient CreateClient(IContext context)
        {
            HttpClient httpClient = new HttpClientBuilder()
                    .WithHandler(new RequestIdMessageRequestHandler())
                    .WithHandler(new SessionIdMessageRequestHandler())
                    .WithDefaultHeaders()
                    .Build();

            return httpClient;
        }
    }
}
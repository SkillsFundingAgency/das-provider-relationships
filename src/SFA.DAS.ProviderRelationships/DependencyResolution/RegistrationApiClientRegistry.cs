using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.Http;
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

    //copied these classes to break dependency on old logging package (which uses asp.net fwk)
    public class RequestIdMessageRequestHandler : DelegatingHandler
    {
        public RequestIdMessageRequestHandler()
        {
        }

        public RequestIdMessageRequestHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string str = MappedDiagnosticsLogicalContext.Get("DasRequestCorrelationId");
            request.Headers.Add("DasRequestCorrelationId", str);
            return base.SendAsync(request, cancellationToken);
        }
    }

    public class SessionIdMessageRequestHandler : DelegatingHandler
    {
        public SessionIdMessageRequestHandler()
        {
        }

        public SessionIdMessageRequestHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string str = MappedDiagnosticsLogicalContext.Get("DasSessionCorrelationId");
            request.Headers.Add("DasSessionCorrelationId", str);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
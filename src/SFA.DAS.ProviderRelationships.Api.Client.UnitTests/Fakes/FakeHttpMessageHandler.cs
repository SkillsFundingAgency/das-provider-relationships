using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Api.Client.UnitTests.Fakes
{
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(HttpResponseMessage ?? throw new InvalidOperationException($"Value for {nameof(HttpResponseMessage)} cannot be null"));
        }
    }
}
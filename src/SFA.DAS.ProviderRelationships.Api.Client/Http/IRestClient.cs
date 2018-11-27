using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    public interface IRestClient
    {
        Task<string> Get(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<string> Get(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(string uri, object queryData = null, CancellationToken cancellationToken = default);
    }
}
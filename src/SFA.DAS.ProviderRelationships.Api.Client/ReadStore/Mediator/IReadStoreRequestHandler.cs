using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator
{
    internal interface IReadStoreRequestHandler<in TRequest, TResponse> where TRequest : IReadStoreRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
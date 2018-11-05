using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    internal interface IApiRequestHandler<in TRequest, TResponse> where TRequest : IApiRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
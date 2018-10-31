using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    internal interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
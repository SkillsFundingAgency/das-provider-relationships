using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    public interface IApiMediator
    {
        Task<TResponse> Send<TResponse>(IApiRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
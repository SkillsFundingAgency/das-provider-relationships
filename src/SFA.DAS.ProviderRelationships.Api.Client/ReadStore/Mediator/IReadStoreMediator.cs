using System.Threading;
using System.Threading.Tasks;

//todo: this mediator will be extracted out into its own nuget package, or we'll switch to MediatR
namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator
{
    public interface IReadStoreMediator
    {
        Task<TResponse> Send<TResponse>(IReadStoreRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
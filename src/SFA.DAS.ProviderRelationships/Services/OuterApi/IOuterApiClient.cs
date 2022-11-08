using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi;

public interface IOuterApiClient
{
    Task<TResponse> Get<TResponse>(IGetApiRequest request);
}
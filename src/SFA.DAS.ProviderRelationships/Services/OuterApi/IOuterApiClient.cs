using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi
{
    public interface IOuterApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
    }
}
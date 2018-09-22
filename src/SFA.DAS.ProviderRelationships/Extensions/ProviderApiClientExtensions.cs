using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class ProviderApiClientExtensions
    {
        public static async Task<Provider> TryGetAsync(this IProviderApiClient client, string ukprn)
        {
            try
            {
                return await client.GetAsync(ukprn);
            }
            catch (EntityNotFoundException)
            {
                return null;
            }
        }
    }
}
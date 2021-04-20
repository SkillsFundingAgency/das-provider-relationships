using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRoatpService
    {
        Task<bool> Ping();
        Task<IEnumerable<ProviderRegistration>> GetProviders();
    }
}
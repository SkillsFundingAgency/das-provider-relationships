using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RoatpService : IRoatpService
    {
        public Task<bool> Ping()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ProviderRegistration>> GetProviders()
        {
            throw new System.NotImplementedException();
        }
    }
}
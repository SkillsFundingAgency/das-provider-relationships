using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Repositories
{
    //name?? is this how we want to do it??
    public class ProviderRelationshipsRepository
    {
        private readonly IProviderRelationshipsDbContext _providerRelationshipsDbContext;
        
        public ProviderRelationshipsRepository(IProviderRelationshipsDbContext providerRelationshipsDbContext)
        {
            _providerRelationshipsDbContext = providerRelationshipsDbContext;
        }
        
        public async Task Add(Account account)
        {
            _providerRelationshipsDbContext.Accounts.Add(account);

            await _providerRelationshipsDbContext.SaveChangesAsync();
        }
    }
}
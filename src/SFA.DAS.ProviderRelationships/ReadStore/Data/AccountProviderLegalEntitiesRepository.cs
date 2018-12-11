using Microsoft.Azure.Documents;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    public class AccountProviderLegalEntitiesRepository : DocumentRepository<AccountProviderLegalEntity>, IAccountProviderLegalEntitiesRepository
    {
        public AccountProviderLegalEntitiesRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
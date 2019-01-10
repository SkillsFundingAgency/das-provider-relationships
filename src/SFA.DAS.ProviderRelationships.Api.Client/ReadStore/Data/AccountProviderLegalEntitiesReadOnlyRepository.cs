using Microsoft.Azure.Documents;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    internal class AccountProviderLegalEntitiesReadOnlyRepository : ReadOnlyDocumentRepository<AccountProviderLegalEntityDto>, IAccountProviderLegalEntitiesReadOnlyRepository
    {
        public AccountProviderLegalEntitiesReadOnlyRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
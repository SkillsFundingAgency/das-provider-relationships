using Microsoft.Azure.Documents;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    internal class AccountProviderLegalEntitiesRepository : DocumentRepository<AccountProviderLegalEntityDto>, IAccountProviderLegalEntitiesRepository
    {
        public AccountProviderLegalEntitiesRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
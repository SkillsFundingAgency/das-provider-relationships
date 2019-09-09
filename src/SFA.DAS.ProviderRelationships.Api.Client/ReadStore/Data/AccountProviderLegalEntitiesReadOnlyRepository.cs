using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    internal class AccountProviderLegalEntitiesReadOnlyRepository : ReadOnlyDocumentRepository<AccountProviderLegalEntityDto>, IAccountProviderLegalEntitiesReadOnlyRepository
    {
        public AccountProviderLegalEntitiesReadOnlyRepository(IDocumentClientFactory documentClientFactory)
            : base(documentClientFactory.CreateDocumentClient(), DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
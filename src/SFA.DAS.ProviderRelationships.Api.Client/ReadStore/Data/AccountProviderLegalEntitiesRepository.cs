using Microsoft.Azure.Documents;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    internal class AccountProviderLegalEntitiesRepository : DocumentRepository<AccountProviderLegalEntityDto>, IAccountProviderLegalEntitiesRepository
    {
        //todo: rename other repo to include "WriteEnabled" or some such, so they have different names

        public AccountProviderLegalEntitiesRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    //todo: create a ReadOnlyDocumentRepository?
    
    internal class AccountProviderLegalEntitiesRepository : ReadOnlyDocumentRepository<AccountProviderLegalEntityDto>, IAccountProviderLegalEntitiesRepository
    {
        //todo: rename other repo to include "WriteEnabled" or some such, so they have different names

        public AccountProviderLegalEntitiesRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    //todo: can we have a repository of the interface? when tried it complained about cross-partition queries!?
    internal class AccountProviderLegalEntitiesReadOnlyRepository : ReadOnlyDocumentRepository<AccountProviderLegalEntityDto>, IAccountProviderLegalEntitiesReadOnlyRepository
    {
        public AccountProviderLegalEntitiesReadOnlyRepository(IDocumentClient documentClient)
            : base(documentClient, DocumentSettings.DatabaseName, DocumentSettings.AccountProviderLegalEntitiesCollectionName)
        {
        }
    }
}
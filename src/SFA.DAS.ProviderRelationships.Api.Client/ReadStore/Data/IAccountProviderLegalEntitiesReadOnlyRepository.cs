using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    internal interface IAccountProviderLegalEntitiesReadOnlyRepository : IReadOnlyDocumentRepository<AccountProviderLegalEntityDto>
    {
    }
}
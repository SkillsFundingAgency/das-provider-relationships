using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    internal interface IAccountProviderLegalEntitiesRepository : IDocumentRepository<AccountProviderLegalEntityDto>
    {
    }
}
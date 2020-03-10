using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class FindAllProvidersQuery : IRequest<FindAllProvidersQueryResult>
    {
        public FindAllProvidersQuery()
        {

        }
    }
}
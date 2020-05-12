using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class GetAllProvidersQuery : IRequest<GetAllProvidersQueryResult>
    {
        public GetAllProvidersQuery()
        {

        }
    }
}
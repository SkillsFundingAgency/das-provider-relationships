using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
using SFA.DAS.ProviderRelationships.Document.Repository.ProviderPermissions;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class GetAnyProviderRelationshipWithThisPermissonHandler //: IRequestHandler<GetAnyProviderRelationshipWithThisPermissonQuery, bool>
    {
        //private readonly Lazy<ProviderRelationshipsDbContext> _db;
        //private readonly IConfigurationProvider _configurationProvider;

        public GetAnyProviderRelationshipWithThisPermissonHandler(IProviderPermissionsReadService reader)
        {
        }

        //public Task<bool> Handle(GetAnyProviderRelationshipWithThisPermissonQuery request, CancellationToken cancellationToken)
        //{
        //    //var healthCheck = await _db.Value.HealthChecks
        //    //    .OrderByDescending(h => h.Id)
        //    //    .ProjectTo<HealthCheckDto>(_configurationProvider)
        //    //    .FirstOrDefaultAsync(cancellationToken);

        //    //return new GetHealthCheckQueryResponse
        //    //{
        //    //    HealthCheck = healthCheck
        //    //};
        //    return ;
        //}
    }
}
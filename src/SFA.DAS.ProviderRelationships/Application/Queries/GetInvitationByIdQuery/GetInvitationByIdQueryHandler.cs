using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery
{
    public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public GetInvitationByIdQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetInvitationByIdQueryResult> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
        {
            var invitation = await _db.Value.Invitations
                .Where(i => i.Reference == request.CorrelationId)
                .ProjectTo<InvitationDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return invitation == null ? null : new GetInvitationByIdQueryResult(invitation);
        }
    }
}
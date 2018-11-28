using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdateAccountLegalEntityNameCommandHandler : AsyncRequestHandler<UpdateAccountLegalEntityNameCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public UpdateAccountLegalEntityNameCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(UpdateAccountLegalEntityNameCommand request, CancellationToken cancellationToken)
        {
            var accountLegalEntity = await _db.Value.AccountLegalEntities.IgnoreQueryFilters().SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

            accountLegalEntity.UpdateName(request.Name, request.Created);
        }
    }
}
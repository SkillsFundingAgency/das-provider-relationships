using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages.Events;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class SetPermissionsCommandHandler : AsyncRequestHandler<SetPermissionsCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IMessageSession _messageSession;

        public SetPermissionsCommandHandler(Lazy<ProviderRelationshipsDbContext> db, IMessageSession messageSession)
        {
            _db = db;
            _messageSession = messageSession;
        }

        protected override async Task Handle(SetPermissionsCommand request, CancellationToken cancellationToken)
        {
            var db = _db.Value;

            var existingPermissionTypes = db.Permissions.Where(
                p => p.AccountLegalEntityId == request.AccountLegalEntityId
                && p.Ukprn == request.Ukprn).Select(p => p.Type).ToArray();

            var permissionsByGranted = request.Permissions.ToLookup(p => p.Granted);

            var grantedPermissionTypes = permissionsByGranted[true].Select(p => p.Type).Except(existingPermissionTypes);

            var revokedPermissionTypes = existingPermissionTypes.Except(permissionsByGranted[false].Select(p => p.Type));

            //todo: cancellationToken?

            //todo whenall everything at the end and return the task non-awaited?

            //use deleteasync or not required as already loaded existing permissions?
            if (revokedPermissionTypes.Any())
            {
                await db.Permissions.Where(
                    p => p.AccountLegalEntityId == request.AccountLegalEntityId
                         && p.Ukprn == request.Ukprn
                         && revokedPermissionTypes.Contains(p.Type)).DeleteAsync(cancellationToken);
            }

            if (grantedPermissionTypes.Any())
            {
                await db.Permissions.AddRangeAsync(grantedPermissionTypes.Select(pt =>
                    new Models.Permission(request.AccountLegalEntityId, request.Ukprn, pt)));
            }
            
            var grantedEventTasks = permissionsByGranted[true].Select(p => _messageSession.Publish(
                new GrantedPermissionEvent(1, request.AccountLegalEntityId, request.Ukprn, (short)p.Type, request.UserName, request.UserRef, DateTime.UtcNow)));

            var revokedEventTasks = permissionsByGranted[false].Select(p => _messageSession.Publish(
                new RevokedPermissionEvent(1, request.AccountLegalEntityId, request.Ukprn, (short)p.Type, request.UserName, request.UserRef, DateTime.UtcNow)));

            await Task.WhenAll(grantedEventTasks.Concat(revokedEventTasks));
            //Parallel.ForEach()?
        }
    }
}
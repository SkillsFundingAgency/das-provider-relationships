using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.Document.Repository;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class PermissionsRepository : DocumentRepository<Permission>, IPermissionsRepository
    {
        public PermissionsRepository(IDocumentClient documentClient)
            : base(documentClient, "SFA.DAS.ProviderRelationships.ReadStore.Database", "permissions")
        {
        }

        public override IQueryable<Permission> CreateQuery(FeedOptions options = null)
        {
            if (options == null)
            {
                options = new FeedOptions {EnableCrossPartitionQuery = false, MaxItemCount = -1};
            }

            return base.CreateQuery(options);
        }

        public Task Update(Permission permission, CancellationToken token = default)
        {
            return Update(permission, null, token);
        }

        public Task UpdateOld(Permission permission, CancellationToken token = default)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));
            if (permission.Id == null) throw new Exception("Entity's Id must contain a Guid when replacing a document");

            var requestOptions = new RequestOptions();

            if (string.IsNullOrEmpty(permission.ETag))
            {
                requestOptions.AccessCondition = new AccessCondition {
                    Condition = permission.ETag,
                    Type = AccessConditionType.IfMatch
                };
            }
            return Update(permission, permission.Id.Value, requestOptions, token);
        }
    }
}
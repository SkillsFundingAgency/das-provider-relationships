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
    }
}
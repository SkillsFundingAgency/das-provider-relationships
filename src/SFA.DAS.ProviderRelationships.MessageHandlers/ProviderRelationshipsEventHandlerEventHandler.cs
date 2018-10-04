using System;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class ProviderRelationshipsEventHandler
    {
        protected readonly Lazy<ProviderRelationshipsDbContext> Db;

        protected ProviderRelationshipsEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            Db = db;
        }
    }
}

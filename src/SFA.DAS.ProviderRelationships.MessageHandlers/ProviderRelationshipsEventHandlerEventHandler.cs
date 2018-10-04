using System;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class ProviderRelationshipsEventHandler
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _lazyDb;
        protected readonly ILog Log;

        protected ProviderRelationshipsDbContext Db => _lazyDb.Value;

        protected ProviderRelationshipsEventHandler(Lazy<ProviderRelationshipsDbContext> db, ILog log)
        {
            _lazyDb = db;
            Log = log;
        }
    }
}

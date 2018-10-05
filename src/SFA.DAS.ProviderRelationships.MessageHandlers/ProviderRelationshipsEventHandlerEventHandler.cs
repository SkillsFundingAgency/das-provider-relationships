using System;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class ProviderRelationshipsEventHandler
    {
        private readonly Lazy<IProviderRelationshipsDbContext> _lazyDb;
        protected readonly ILog Log;

        protected IProviderRelationshipsDbContext Db => _lazyDb.Value;

        protected ProviderRelationshipsEventHandler(Lazy<IProviderRelationshipsDbContext> db, ILog log)
        {
            _lazyDb = db;
            Log = log;
        }
    }
}

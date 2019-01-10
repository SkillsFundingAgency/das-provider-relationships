using System;
using SFA.DAS.CosmosDb;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public static class DocumentActivator
    {
        public static T CreateInstance<T>() where T : IDocument
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}
using System;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders
{
    public static class DocumentActivator
    {
        internal static T CreateInstance<T>() where T : Document
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}
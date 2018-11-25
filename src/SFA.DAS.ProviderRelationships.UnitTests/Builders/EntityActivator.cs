using System;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public static class EntityActivator
    {
        public static T CreateInstance<T>() where T : Entity
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}
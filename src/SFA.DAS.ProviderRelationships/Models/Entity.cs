using System;
using SFA.DAS.NServiceBus;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Models
{
    public abstract class Entity
    {
        protected void Publish<T>(Func<T> action) where T : Event
        {
            UnitOfWorkContext.AddEvent(action);
        }
    }
}
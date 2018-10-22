using System;

namespace SFA.DAS.ProviderRelationships.Exceptions
{
    public class MissingEntityException/*<TEntity>*/ : Exception
    {
        public MissingEntityException(string message)
            : base(message)
        {
        }
    }
}
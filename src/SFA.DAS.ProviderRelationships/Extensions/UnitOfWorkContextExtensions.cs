using System;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class UnitOfWorkContextExtensions
    {
        //todo: TryGet has been removed from IUnitOfWorkContext for some reason. find out why. add back?
        public static T TryGet<T>(this IUnitOfWorkContext context) where T : class
        {
            try
            {
                return context.Get<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Web.Data;
using SFA.DAS.UnitOfWork;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public class EntityFrameworkCoreUnitOfWorkRegistry<T> : Registry where T : DbContext
    {
        public EntityFrameworkCoreUnitOfWorkRegistry()
        {
            For<IUnitOfWork>().Add<EntityFrameworkCoreUnitOfWork<T>>();
        }
    }
}
using NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using StructureMap;
using BatchDeleteRelationshipsCommandHandler = SFA.DAS.ProviderRelationships.MessageHandlers.CommandHandlers.BatchDeleteRelationshipsCommandHandler;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IHandleMessages<BatchDeleteRelationshipsCommand>>().Use<BatchDeleteRelationshipsCommandHandler>();
        }
    }
}
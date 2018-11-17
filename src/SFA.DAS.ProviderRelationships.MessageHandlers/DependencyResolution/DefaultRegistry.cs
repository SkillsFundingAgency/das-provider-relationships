using NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using StructureMap;
using BatchUpdateRelationshipAccountNamesCommandHandler = SFA.DAS.ProviderRelationships.MessageHandlers.CommandHandlers.BatchUpdateRelationshipAccountNamesCommandHandler;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IHandleMessages<BatchUpdateRelationshipAccountNamesCommand>>().Use<BatchUpdateRelationshipAccountNamesCommandHandler>();
        }
    }
}
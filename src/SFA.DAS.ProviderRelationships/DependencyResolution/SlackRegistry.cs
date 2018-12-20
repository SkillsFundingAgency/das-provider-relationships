using SFA.DAS.ProviderRelationships.Slack;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class SlackRegistry : Registry
    {
        public SlackRegistry()
        {
            For<ISlackClient>().Use<SlackClient>();
        }
    }
}
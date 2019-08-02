using SFA.DAS.Encoding;
using SFA.DAS.HashingService;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class HashingRegistry : Registry
    {
        public HashingRegistry()
        {
            For<IHashingService>().Use(c => GetHashingService(c));
            For<IEncodingService>().Use<EncodingService>();
        }

        private IHashingService GetHashingService(IContext context)
        {
            var config = context.GetInstance<ProviderRelationshipsConfiguration>();
            var hashingService = new HashingService.HashingService(config.AllowedHashstringCharacters, config.Hashstring);

            return hashingService;
        }
    }
}
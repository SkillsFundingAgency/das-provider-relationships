using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests
{
    public class ProviderBuilder
    {
        private readonly Mock<Provider> _provider = new Mock<Provider> { CallBase = true };

        public ProviderBuilder WithUkprn(long ukprn)
        {
            _provider.SetupProperty(p => p.Ukprn, ukprn);

            return this;
        }

        public ProviderBuilder WithName(string name)
        {
            _provider.SetupProperty(p => p.Name, name);
            
            return this;
        }

        public Provider Build()
        {
            return _provider.Object;
        }
    }
}
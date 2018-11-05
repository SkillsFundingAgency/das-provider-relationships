using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    public class SearchProvidersQueryResponseTests : FluentTest<object>
    {
        [Test]
        public void ProviderFound_WhenAProviderIdIsNotNull_ThenShouldReturnTrue()
        {
            Run(f => new SearchProvidersQueryResponse(12345678, null), (f, r) => r.ProviderFound.Should().BeTrue());
        }
        
        [Test]
        public void ProviderFound_WhenAProviderIdIsNull_ThenShouldReturnTrue()
        {
            Run(f => new SearchProvidersQueryResponse(null, null), (f, r) => r.ProviderFound.Should().BeFalse());
        }
        
        [Test]
        public void ProviderAlreadyAdded_WhenAAccountProviderIdIsNotNull_ThenShouldReturnTrue()
        {
            Run(f => new SearchProvidersQueryResponse(null, 1), (f, r) => r.ProviderAlreadyAdded.Should().BeTrue());
        }
        
        [Test]
        public void ProviderAlreadyAdded_WhenAAccountProviderIdIsNull_ThenShouldReturnTrue()
        {
            Run(f => new SearchProvidersQueryResponse(null, null), (f, r) => r.ProviderAlreadyAdded.Should().BeFalse());
        }
    }
}
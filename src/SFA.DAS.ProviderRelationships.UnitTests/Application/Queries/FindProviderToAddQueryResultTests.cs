using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class FindProviderToAddQueryResultTests : FluentTest<object>
    {
        [Test]
        public void ProviderNotFound_WhenProviderIdIsNotNull_ThenShouldReturnFalse()
        {
            Run(f => new FindProviderToAddQueryResult(12345678, null), (f, r) => r.ProviderNotFound.Should().BeFalse());
        }
        
        [Test]
        public void ProviderNotFound_WhenProviderIdIsNull_ThenShouldReturnTrue()
        {
            Run(f => new FindProviderToAddQueryResult(null, null), (f, r) => r.ProviderNotFound.Should().BeTrue());
        }
        
        [Test]
        public void ProviderAlreadyAdded_WhenAccountProviderIdIsNotNull_ThenShouldReturnTrue()
        {
            Run(f => new FindProviderToAddQueryResult(null, 1), (f, r) => r.ProviderAlreadyAdded.Should().BeTrue());
        }
        
        [Test]
        public void ProviderAlreadyAdded_WhenAccountProviderIdIsNull_ThenShouldReturnTrue()
        {
            Run(f => new FindProviderToAddQueryResult(null, null), (f, r) => r.ProviderAlreadyAdded.Should().BeFalse());
        }
    }
}
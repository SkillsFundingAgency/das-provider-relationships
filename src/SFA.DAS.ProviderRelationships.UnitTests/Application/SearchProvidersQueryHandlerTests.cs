using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;
using SFA.DAS.Validation;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application
{
    [TestFixture]
    public class SearchProvidersQueryHandlerTests : FluentTest<SearchProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndAProviderIsFound_ThenShouldReturnASearchProvidersQueryResponse()
        {
            return RunAsync(f => f.SetProviderResponse(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Ukprn.Should().Be(int.Parse(f.Query.Ukprn));
            });
        }

        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndAProviderIsNotFound_ThenShouldThrowAValidationException()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().Throw<ValidationException>());
        }
    }

    public class SearchProvidersQueryHandlerTestsFixture
    {
        public SearchProvidersQueryHandler Handler { get; set; }
        public SearchProvidersQuery Query { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }

        public SearchProvidersQueryHandlerTestsFixture()
        {
            ProviderApiClient = new Mock<IProviderApiClient>();
            Handler = new SearchProvidersQueryHandler(ProviderApiClient.Object);

            Query = new SearchProvidersQuery
            {
                Ukprn = "12345678"
            };
        }

        public Task<SearchProvidersQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public SearchProvidersQueryHandlerTestsFixture SetProviderResponse()
        {
            ProviderApiClient.Setup(c => c.ExistsAsync(Query.Ukprn)).ReturnsAsync(true);

            return this;
        }
    }
}
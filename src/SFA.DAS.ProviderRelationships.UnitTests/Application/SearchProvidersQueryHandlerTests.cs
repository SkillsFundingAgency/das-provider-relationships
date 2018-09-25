using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;
using SFA.DAS.Validation;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application
{
    [TestFixture]
    public class SearchProvidersQueryHandlerTests : FluentTest<SearchProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndAProviderIsFound_ThenShouldReturnTheProvidersDetails()
        {
            return RunAsync(f => f.SetProviderResponse(), f => f.Handle(), (f, r) => r.Provider.Should().NotBeNull()
                .And.Match<ProviderDto>(d => d.Ukprn == f.ProviderResponse.Ukprn && d.Name == f.ProviderResponse.ProviderName));
        }

        [Test]
        public Task Handle_WhenHandlingASearchProvidersQueryAndAProviderIsNotFound_ThenShouldThrowAValidationException()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.ShouldThrow<ValidationException>());
        }
    }

    public class SearchProvidersQueryHandlerTestsFixture
    {
        public SearchProvidersQueryHandler Handler { get; set; }
        public SearchProvidersQuery Query { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }
        public IMapper Mapper { get; set; }
        public Provider ProviderResponse { get; set; }

        public SearchProvidersQueryHandlerTestsFixture()
        {
            ProviderApiClient = new Mock<IProviderApiClient>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();
            Handler = new SearchProvidersQueryHandler(ProviderApiClient.Object, Mapper);

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
            ProviderResponse = new Provider
            {
                Ukprn = int.Parse(Query.Ukprn),
                ProviderName = "Foo"
            };

            ProviderApiClient.Setup(c => c.GetAsync(Query.Ukprn)).ReturnsAsync(ProviderResponse);

            return this;
        }
    }
}
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
    public class SearchTrainingProvidersQueryHandlerTests : FluentTest<SearchTrainingProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingASearchTrainingProvidersQueryAndATrainingProviderIsFound_ThenShouldReturnTheTrainingProvidersDetails()
        {
            return RunAsync(f => f.SetTrainingProviderResponse(), f => f.Handle(), (f, r) => r.TrainingProvider.Should().NotBeNull()
                .And.Match<TrainingProviderDto>(d => d.Ukprn == f.ProviderResponse.Ukprn && d.Name == f.ProviderResponse.ProviderName));
        }

        [Test]
        public Task Handle_WhenHandlingASearchTrainingProvidersQueryAndATrainingProviderIsNotFound_ThenShouldThrowAValidationException()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.ShouldThrow<ValidationException>());
        }
    }

    public class SearchTrainingProvidersQueryHandlerTestsFixture
    {
        public SearchTrainingProvidersQueryHandler Handler { get; set; }
        public SearchTrainingProvidersQuery Query { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }
        public IMapper Mapper { get; set; }
        public Provider ProviderResponse { get; set; }

        public SearchTrainingProvidersQueryHandlerTestsFixture()
        {
            ProviderApiClient = new Mock<IProviderApiClient>();
            Mapper = new MapperConfiguration(c => c.AddProfile<TrainingProviderMappings>()).CreateMapper();
            Handler = new SearchTrainingProvidersQueryHandler(ProviderApiClient.Object, Mapper);

            Query = new SearchTrainingProvidersQuery
            {
                Ukprn = "12345678"
            };
        }

        public Task<SearchTrainingProvidersQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public SearchTrainingProvidersQueryHandlerTestsFixture SetTrainingProviderResponse()
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
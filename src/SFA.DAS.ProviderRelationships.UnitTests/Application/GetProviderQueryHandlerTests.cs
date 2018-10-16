using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class GetProviderQueryHandlerTests : FluentTest<GetProviderQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAGetProviderQueryAndAProviderIsFound_ThenShouldReturnAGetProviderQueryResponse()
        {
            return RunAsync(f => f.SetProviderResponse(), f => f.Handle(), (f, r) => r.Should().NotBeNull()
                .And.Match<GetProviderQueryResponse>(r2 =>
                    r2.Provider.Ukprn == f.ProviderResponse.Ukprn &&
                    r2.Provider.Name == f.ProviderResponse.ProviderName));
        }

        [Test]
        public Task Handle_WhenHandlingAGetProviderQueryAndAProviderIsNotFound_ThenShouldReturnAGetProviderQueryResponseWithNullProvider()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeNull());
        }
    }

    public class GetProviderQueryHandlerTestsFixture
    {
        public GetProviderQueryHandler Handler { get; set; }
        public GetProviderQuery Query { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }
        public IMapper Mapper { get; set; }
        public Provider ProviderResponse { get; set; }

        public GetProviderQueryHandlerTestsFixture()
        {
            ProviderApiClient = new Mock<IProviderApiClient>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();
            Handler = new GetProviderQueryHandler(ProviderApiClient.Object, Mapper);

            Query = new GetProviderQuery
            {
                Ukprn = "12345678"
            };
        }

        public Task<GetProviderQueryResponse> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetProviderQueryHandlerTestsFixture SetProviderResponse()
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
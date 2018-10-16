using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.EntityFramework;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class GetHealthCheckQueryHandlerTests : FluentTest<GetHealthCheckQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAGetHealthCheckQuery_ThenShouldReturnAGetHealthCheckQueryResponse()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.HealthCheck.Should().NotBeNull().And.Match<HealthCheckDto>(d => d.Id == f.HealthChecks[1].Id);
            });
        }
    }

    public class GetHealthCheckQueryHandlerTestsFixture
    {
        public GetHealthCheckQuery GetHealthCheckQuery { get; set; }
        public IRequestHandler<GetHealthCheckQuery, GetHealthCheckQueryResponse> Handler { get; set; }
        public Mock<IProviderRelationshipsDbContext> Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }
        public List<HealthCheck> HealthChecks { get; set; }

        public GetHealthCheckQueryHandlerTestsFixture()
        {
            GetHealthCheckQuery = new GetHealthCheckQuery();
            Db = new Mock<IProviderRelationshipsDbContext>();
            ConfigurationProvider = new MapperConfiguration(c => c.AddProfile<HealthCheckMappings>());

            HealthChecks = new List<HealthCheck>
            {
                new HealthCheckBuilder().WithId(1).Build(),
                new HealthCheckBuilder().WithId(2).Build()
            };

            Db.Setup(d => d.HealthChecks).Returns(new DbSetStub<HealthCheck>(HealthChecks));

            Handler = new GetHealthCheckQueryHandler(new Lazy<IProviderRelationshipsDbContext>(() => Db.Object), ConfigurationProvider);
        }

        public Task<GetHealthCheckQueryResponse> Handle()
        {
            return Handler.Handle(GetHealthCheckQuery, CancellationToken.None);
        }
    }
}
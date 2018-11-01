using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Testing;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class HasRelationshipWithPermissionQueryHandlerTests : FluentTest<HasRelationshipWithPermissionQueryHandlerTestsFixture>
    {
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public Task Handle_WhenHandlingHasRelationshipWithPermissionQuery_ThenShouldReturnHasRelationshipWithPermissionQueryResponse(long ukprn, bool result)
        {
            return RunAsync(f => f.Handle(ukprn), (f, r) => r.Should().Be(result));
        }
    }

    public class HasRelationshipWithPermissionQueryHandlerTestsFixture
    {
        internal IRequestHandler<HasRelationshipWithPermissionQuery, bool> Handler { get; set; }
        public Mock<IReadOnlyDocumentRepository<Permission>> ReadOnlyDocumentRepository { get; set; }
        public IOrderedQueryable<Permission> DocumentQuery { get; set; }
        public List<Permission> Permissions { get; set; }

        public HasRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            Permissions = new List<Permission>
            {
                new PermissionBuilder().WithUkprn(1).WithOperation(Operation.CreateCohort).Build(),
                new PermissionBuilder().WithUkprn(1).Build(),
                new PermissionBuilder().WithUkprn(2).WithOperation(Operation.CreateCohort).Build(),
                new PermissionBuilder().WithUkprn(2).WithOperation(Operation.CreateCohort).Build(),
                new PermissionBuilder().WithUkprn(2).Build(),
                new PermissionBuilder().WithUkprn(3).Build(),
            };

            ReadOnlyDocumentRepository = new Mock<IReadOnlyDocumentRepository<Permission>>();
            DocumentQuery = new DocumentQueryBuilder<Permission>().WithDocuments(Permissions).Build();

            ReadOnlyDocumentRepository.Setup(r => r.CreateQuery()).Returns(DocumentQuery);

            Handler = new HasRelationshipWithPermissionQueryHandler(ReadOnlyDocumentRepository.Object);
        }

        public Task<bool> Handle(long ukprn)
        {
            return Handler.Handle(new HasRelationshipWithPermissionQuery(ukprn, Operation.CreateCohort), CancellationToken.None);
        }
    }
}
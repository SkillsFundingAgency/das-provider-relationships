using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Data
{
    [TestFixture]
    [Parallelizable]
    public class PermissionRepositoryTests : FluentTest<PermissionRepositoryTestsFixture>
    {
        [Test]
        public void CreateUkprnQuery_WhenCalled_ThenSetsCorrectFeedOptions()
        {
            Run(f => f.PermissionsRepository.CreateQuery(), (f,r) => r.Should().BeSameAs(f.OrderedQueryable.Object));
        }

        [Test]
        public Task Update_WhenPermissionIsNull_ThenShouldThrowException()
        {
            return RunAsync(f => f.PermissionsRepository.Update(null, null, CancellationToken.None), (f, r) => r.Should().Throw<Exception>());
        }

        [Test]
        public Task Update_WhenPermissionsIdIsNull_ThenShouldThrowException()
        {
            return RunAsync(f => f.CreateSinglePermissionWithIdAndETag(null, null), 
                f => f.PermissionsRepository.Update(f.SinglePermission, null, CancellationToken.None), 
                (f, r) => r.Should().Throw<Exception>());
        }

        //[Test]
        //public Task Update_WhenPermissionsIdIsAGuidAndNoETag_ThenShouldCreateADocumentWithNoFeedOptions()
        //{
        //    return RunAsync(f => f.CreateSinglePermissionWithIdAndETag(Guid.NewGuid(), null),
        //        f => f.PermissionsRepository.Update(f.SinglePermission, CancellationToken.None),
        //        (f, r) =>
        //        {
        //            f.DocumentClient.Verify(x=>x.ReplaceDocumentAsync(It.IsAny<Uri>(), It.Is<Permission>(p=>p == f.SinglePermission), 
        //                It.Is<RequestOptions>(p=>p.AccessCondition == null), It.IsAny<CancellationToken>()));
        //            return r;
        //        });
        //}

    }

    public class PermissionRepositoryTestsFixture
    {
        internal Permission SinglePermission;
        internal Mock<IOrderedQueryable<Permission>> OrderedQueryable;
        internal PermissionsRepository PermissionsRepository { get; set; }
        internal Mock<IDocumentClient> DocumentClient { get; set; }

        public PermissionRepositoryTestsFixture()
        {
            OrderedQueryable = new Mock<IOrderedQueryable<Permission>>();
            DocumentClient = new Mock<IDocumentClient>();
            DocumentClient.Setup(x => x.CreateDocumentQuery<Permission>(It.IsAny<Uri>(),
                    It.Is<FeedOptions>(p => p.EnableCrossPartitionQuery == false && p.MaxItemCount == -1)))
                .Returns(OrderedQueryable.Object);
            PermissionsRepository = new PermissionsRepository(DocumentClient.Object);
        }

        public void Test()
        {
            //return this;
        }

        public PermissionRepositoryTestsFixture CreateSinglePermissionWithIdAndETag(Guid? id, string eTag)
        {
            SinglePermission = new PermissionBuilder()
                .WithId(id)
                .WithETag(eTag)
                .WithAccountId(1)
                .WithAccountPublicHashedId("AAA111")
                .WithAccountName("account name 1")
                .WithAccountLegalEntityId(1)
                .WithAccountLegalEntityPublicHashedId("ALE111")
                .WithAccountLegalEntityName("legal entity name ALE111")
                .WithAccountProviderId(1)
                .WithUkprn(11111111)
                .WithOperation(Operation.CreateCohort)
                .Build();

            return this;
        }

    }
}
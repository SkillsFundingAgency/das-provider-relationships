using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.RelationshipsController
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndRelationshipExistsForProviderAndTheyHavePermissionForOperation_ThenShouldReturnCorrectRelationship()
        {
        }

        //todo: distinguish between not founds (put error message in response body indicating what was not found)? return empty?
        //todo: in general, include error response in body, something like {ErrorCode: x, ErrorMessage: ""}
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndRelationshipExistsForProviderAndTheyDoNotHavePermissionForOperation_ThenShouldReturnNotFound()
        {
        }
        
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndRelationshipDoesNotExistsForProvider_ThenShouldReturnNotFound()
        {
        }
        
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndProviderDoesntExist_ThenShouldReturnNotFound()
        {
        }
        
        [Test]
        public void WhenUkprIsMissing_ThenShouldReturnNotImplemented()
        {
        }
        
        [Test]
        public void WhenOperationIsMissing_ThenShouldReturnNotImplemented()
        {
        }

        [Test]
        public void WhenUnknownOperationIsSupplied_ThenShouldReturnBadRequest()
        {
        }
    }

    public class GetTestsFixture
    {
    }
}
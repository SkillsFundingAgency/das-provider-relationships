using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using FluentAssertions;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions
{
    public static class IHttpActionResultExtensions
    {
        public static void AssertSingleModelError(this IHttpActionResult result, string propertyName, string expectedErrorMessage)
        {
            result.Should().NotBeNull();
            var modelStateDictionary = result.GetModelStateDictionary();
            modelStateDictionary.Should().NotBeNull();
            modelStateDictionary.HasSingleModelError(propertyName, expectedErrorMessage).Should().BeTrue();
        }

        public static ModelStateDictionary GetModelStateDictionary(this IHttpActionResult result)
        {
            InvalidModelStateResult modelStateResult = result as InvalidModelStateResult;
            if (modelStateResult == null)
                return null;

            ModelStateDictionary modelStateDictionary = modelStateResult.ModelState;
            return modelStateDictionary;
        }
    }
}

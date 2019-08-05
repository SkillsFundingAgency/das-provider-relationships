using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions
{
    public static class ModelStateDictionaryExtensions
    {

        public static bool HasSingleModelError(this ModelStateDictionary modelStateDictionary, string propertyName, string expectedErrorMessage)
        {
            if (modelStateDictionary == null)
                return false;

            if (modelStateDictionary.Count != 1)
                return false;

            KeyValuePair<string, ModelState> firstError = modelStateDictionary.First();

            if (firstError.Key != propertyName)
                return false;

            if (firstError.Value.Errors.Count != 1)
                return false;

            string errorMessage = firstError.Value.Errors.Single().ErrorMessage;
            return errorMessage == expectedErrorMessage;
        }

    }
}

using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions
{
    public static class ModelStateDictionaryExtensions
    {

        public static bool HasModelError(this ModelStateDictionary modelStateDictionary, string propertyName, string expectedErrorMessage)
        {
            if (modelStateDictionary == null)
                return false;

            if (!modelStateDictionary.ContainsKey(propertyName))
                return false;

            var error = modelStateDictionary.Single(m => m.Key == propertyName);

            if (error.Value.Errors.Count != 1)
                return false;

            string errorMessage = error.Value.Errors.Single().ErrorMessage;
            return errorMessage == expectedErrorMessage;
        }

    }
}

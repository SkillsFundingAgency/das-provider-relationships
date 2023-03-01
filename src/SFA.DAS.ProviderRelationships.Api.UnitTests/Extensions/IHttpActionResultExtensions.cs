using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;

public static class HttpActionResultExtensions
{
    public static void AssertModelError(this IActionResult result, string propertyName, string expectedErrorMessage)
    {
        result.Should().NotBeNull();
        var modelStateDictionary = result.GetModelStateDictionary();
        modelStateDictionary.Should().NotBeNull();
        modelStateDictionary.HasModelError(propertyName, expectedErrorMessage).Should().BeTrue();
    }

    public static ModelStateDictionary GetModelStateDictionary(this IActionResult result)
    {
        InvalidModelStateResult modelStateResult = result as InvalidModelStateResult;
        if (modelStateResult == null)
            return null;

        ModelStateDictionary modelStateDictionary = modelStateResult.ModelState;
        return modelStateDictionary;
    }
}
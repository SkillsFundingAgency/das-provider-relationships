using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;

public static class HttpActionResultExtensions
{
    public static void AssertModelError(this IActionResult result, string propertyName, string expectedErrorMessage)
    {
        result.Should().NotBeNull();

        var objectResult = result as BadRequestObjectResult;
        objectResult.Value.GetType().Should().Be<SerializableError>();

        var errors = objectResult.Value as SerializableError;
        errors.ContainsKey(propertyName).Should().BeTrue();

        var errorItem = errors[propertyName] as string[];
        errorItem.Single().Should().Be(expectedErrorMessage);
    }
}
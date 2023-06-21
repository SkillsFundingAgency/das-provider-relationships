using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Extensions;

public class ConfigurationExtensionsTests
{
    [Test]
    [MoqInlineAutoData("true", true)]
    [MoqInlineAutoData("false", false)]
    [MoqInlineAutoData(null, false)]
    [MoqInlineAutoData("", false)]
    public void UseGovUkSignIn_WhenConfigValue_ReturnCorrectValue(string configValue, bool expected)
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x[$"UseGovUkSignIn"]).Returns(configValue);

        // Act
        var result = configuration.Object.UseGovUkSignIn();

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    [MoqInlineAutoData("true", true)]
    [MoqInlineAutoData("false", false)]
    [MoqInlineAutoData(null, false)]
    [MoqInlineAutoData("", false)]
    public void UseStubAuth_WhenConfigValue_ReturnCorrectValue(string configValue, bool expected)
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x["StubAuth"]).Returns(configValue);

        // Act
        var result = configuration.Object.UseStubAuth();

        // Assert
        result.Should().Be(expected);
    }
}
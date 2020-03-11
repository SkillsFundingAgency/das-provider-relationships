using NUnit.Framework;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Extensions
{
    public class HtmlHelperExtensionsTests
    {
        [TestCaseSource(nameof(LabelCases))]
        public void WhenICallSetZenDeskLabelsWithLabels_ThenTheKeywordsAreCorrect(string[] labels, string keywords)
        {
            // Arrange
            var expected = $"<script type=\"text/javascript\">zE('webWidget', 'helpCenter:setSuggestions', {{ labels: [{keywords}] }});</script>";

            // Act
            var actual = Web.Extensions.HtmlHelperExtensions.SetZenDeskLabels(null, labels).ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private static readonly object[] LabelCases =
        {
            new object[] { new string[] { "a string with multiple words", "the title of another page" }, "'a string with multiple words','the title of another page'"},
            new object[] { new string[] { "permissions-training-provider-permissions" }, "'permissions-training-provider-permissions'"},
            new object[] { new string[] { "eas-apostrophe's" }, @"'eas-apostrophe\'s'"},
            new object[] { new string[] { null }, "''" }
        };
    }
}

using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Filters;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Filters
{
    [TestFixture]
    [Parallelizable]
    public class GoogleAnalyticsViewBagFilterTests : FluentTest<GoogleAnalyticsViewBagFilterTestsFixture>
    {
        [Test]
        public void OnActionExecuting_WhenActionIsExecuting_ThenShouldAddGoogleAnalyticsConfigurationToViewBag()
        {
            Run(f => f.OnActionExecuting(), f => (f.ActionExecutingContext.Controller.ViewBag.GoogleAnalyticsConfiguration as GoogleAnalyticsConfiguration).Should().NotBeNull()
                .And.BeSameAs(f.GoogleAnalyticsConfiguration));
        }
    }

    public class GoogleAnalyticsViewBagFilterTestsFixture
    {
        public ActionExecutingContext ActionExecutingContext { get; set; }
        public Mock<ControllerBase> Controller { get; set; }
        public GoogleAnalyticsViewBagFilter GoogleAnalyticsViewBagFilter { get; set; }
        public GoogleAnalyticsConfiguration GoogleAnalyticsConfiguration { get; set; }
        
        public GoogleAnalyticsViewBagFilterTestsFixture()
        {
            Controller = new Mock<ControllerBase>();
            
            ActionExecutingContext = new ActionExecutingContext
            {
                Controller = Controller.Object
            };

            GoogleAnalyticsConfiguration = new GoogleAnalyticsConfiguration();
            GoogleAnalyticsViewBagFilter = new GoogleAnalyticsViewBagFilter(() => GoogleAnalyticsConfiguration);
        }

        public void OnActionExecuting()
        {
            GoogleAnalyticsViewBagFilter.OnActionExecuting(ActionExecutingContext);
        }
    }
}
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class HomeControllerTests : FluentTest<HomeControllerTestsFixture>
    {
        const string ExampleEmployerAccountId = "ABC123";

        [Test]
        public void Index_WhenGettingIndexAction_ThenShouldRedirectToEmployerPortal()
        {
            Run(f => f.SetCurrentEnvironmentIsLocal(false), f => f.Local(), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectResult>(a => a.Url == HomeControllerTestsFixture.EmployerPortalUrl));
        }

        [Test]
        public void Index_WhenGettingIndexActionWithSuppliedEmployerAccountId_AndEnvironmentIsNotLocal_ThenShouldRedirectToAccountProvidersPage()
        {
            Run(f => f.SetCurrentEnvironmentIsLocal(false), f => f.LocalWithEmployerAccountId(ExampleEmployerAccountId), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectResult>(a => a.Url == HomeControllerTestsFixture.EmployerPortalUrl));
        }

        [Test]
        public void Index_WhenGettingIndexActionWithSuppliedEmployerAccountId_AndEnvironmentIsLocal_ThenShouldRedirectToAccountProvidersPage()
        {
            Run(f => f.SetCurrentEnvironmentIsLocal(true), f => f.LocalWithEmployerAccountId(ExampleEmployerAccountId), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectToRouteResult>(rr => rr.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId) && rr.RouteValues.ContainsValue(ExampleEmployerAccountId)));
        }
    }

    public class HomeControllerTestsFixture
    {
        public HomeController HomeController { get; set; }
        public Mock<IEnvironmentService> EnvironmentService { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        
        public const string EmployerPortalUrl = "https://foo.bar";

        public HomeControllerTestsFixture()
        {
            EnvironmentService = new Mock<IEnvironmentService>();
            EmployerUrls = new Mock<IEmployerUrls>();

            EmployerUrls.Setup(au => au.Homepage()).Returns(EmployerPortalUrl);
            
            HomeController = new HomeController(EnvironmentService.Object, EmployerUrls.Object);
        }

        public ActionResult Local()
        {
            return HomeController.Index();
        }

        public ActionResult LocalWithEmployerAccountId(string employerAccountId)
        {
            return HomeController.Index(employerAccountId);
        }

        public HomeControllerTestsFixture SetCurrentEnvironmentIsLocal(bool isLocal)
        {
            EnvironmentService.Setup(e => e.IsCurrent(DasEnv.LOCAL)).Returns(isLocal);
            
            return this;
        }
    }
}
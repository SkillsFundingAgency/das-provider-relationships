using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class HomeControllerTests : FluentTest<HomeControllerTestsFixture>
    {
        [Test]
        public void Index_WhenGettingIndexAction_ThenShouldRedirectToEmployerPortal()
        {
            Run(f => f.SetCurrentEnvironmentIsLocal(false), f => f.Local(), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectResult>(a => a.Url == HomeControllerTestsFixture.EmployerPortalUrl));
        }
    }

    public class HomeControllerTestsFixture
    {
        public HomeController HomeController { get; set; }
        public Mock<IEnvironmentService> Environment { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        
        public const string EmployerPortalUrl = "https://foo.bar";

        public HomeControllerTestsFixture()
        {
            Environment = new Mock<IEnvironmentService>();
            EmployerUrls = new Mock<IEmployerUrls>();

            EmployerUrls.Setup(au => au.Homepage()).Returns(EmployerPortalUrl);
            
            HomeController = new HomeController(Environment.Object, EmployerUrls.Object);
        }

        public ActionResult Local()
        {
            return HomeController.Index();
        }

        public HomeControllerTestsFixture SetCurrentEnvironmentIsLocal(bool isLocal)
        {
            Environment.Setup(e => e.IsCurrent(DasEnv.LOCAL)).Returns(isLocal);
            
            return this;
        }
    }
}
using System.Configuration;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class HomeControllerTests : FluentTest<HomeControllerTestsFixture>
    {
        [Test]
        public void Index_WhenGettingLocalAction_ThenShouldRedirectToEmployerPortal()
        {
            Run(f => f.SetEnvironment(DasEnv.AT), f => f.Local(), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectResult>(a => a.Url == f.Configuration.EmployerPortalBaseUrl));
        }
        
        [Test]
        public void Index_WhenGettingIndexAction_ThenShouldReturnView()
        {
            Run(f => f.SetEnvironment(DasEnv.AT), f => f.Index(), (f, r) => r.Should().NotBeNull()
                .And.Match<ViewResult>(a => a.ViewName == "" && a.Model == null));
        }
    }

    public class HomeControllerTestsFixture
    {
        public Mock<IDependencyResolver> Resolver { get; set; }
        public ProviderRelationshipsConfiguration Configuration { get; set; }
        public HomeController HomeController { get; set; }

        public HomeControllerTestsFixture()
        {
            Resolver = new Mock<IDependencyResolver>();

            Configuration = new ProviderRelationshipsConfiguration
            {
                EmployerPortalBaseUrl = "https://foo.bar"
            };

            Resolver.Setup(r => r.GetService(typeof(ProviderRelationshipsConfiguration))).Returns(Configuration);

            DependencyResolver.SetResolver(Resolver.Object);

            HomeController = new HomeController();
        }

        public ActionResult Index()
        {
            return HomeController.Index();
        }

        public ActionResult Local()
        {
            return HomeController.Local();
        }

        public HomeControllerTestsFixture SetEnvironment(DasEnv environment)
        {
            ConfigurationManager.AppSettings["EnvironmentName"] = environment.ToString();

            return this;
        }
    }
}
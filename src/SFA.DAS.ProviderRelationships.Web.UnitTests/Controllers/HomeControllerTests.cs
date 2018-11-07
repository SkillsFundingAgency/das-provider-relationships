using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Home;
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
        public Task Index_WhenGettingIndexAction_ThenShouldReturnView()
        {
            return RunAsync(f => f.SetEnvironment(DasEnv.AT), f => f.Index(), (f, r) =>
            {
                r.Should().NotBeNull().And.Match<ViewResult>(a => a.ViewName == "");
                r.As<ViewResult>().Model.Should().NotBeNull()
                    .And.BeOfType<AccountProvidersViewModel>()
                    .Which.AccountProviders.Should().BeEquivalentTo(f.GetAccountProvidersQueryResult.AccountProviders);
            });
        }
    }

    public class HomeControllerTestsFixture
    {
        public Mock<IDependencyResolver> Resolver { get; set; }
        public ProviderRelationshipsConfiguration Configuration { get; set; }
        public HomeController HomeController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public AccountProvidersRouteValues AccountProvidersRouteValues { get; set; }
        public GetAccountProvidersQueryResult GetAccountProvidersQueryResult { get; set; }

        public HomeControllerTestsFixture()
        {
            Resolver = new Mock<IDependencyResolver>();

            Configuration = new ProviderRelationshipsConfiguration
            {
                EmployerPortalBaseUrl = "https://foo.bar"
            };

            Resolver.Setup(r => r.GetService(typeof(ProviderRelationshipsConfiguration))).Returns(Configuration);

            DependencyResolver.SetResolver(Resolver.Object);

            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();

            HomeController = new HomeController(Mediator.Object, Mapper);
        }

        public Task<ActionResult> Index()
        {
            AccountProvidersRouteValues = new AccountProvidersRouteValues
            {
                AccountId = 7777777
            };
            
            GetAccountProvidersQueryResult = new GetAccountProvidersQueryResult(new[]
            {
                new AccountProviderDto
                {
                    Id = 666666,
                    ProviderName = "ProviderName"
                }
            });

            Mediator.Setup(m => m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == AccountProvidersRouteValues.AccountId), CancellationToken.None))
                .ReturnsAsync(GetAccountProvidersQueryResult);
            
            return HomeController.Index(AccountProvidersRouteValues);
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
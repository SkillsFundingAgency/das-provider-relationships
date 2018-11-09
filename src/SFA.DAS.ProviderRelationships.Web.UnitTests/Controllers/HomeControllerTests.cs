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
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.Mappings;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Home;
using SFA.DAS.Testing;
using Fix = SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers.HomeControllerTestsFixture;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class HomeControllerTests : FluentTest<HomeControllerTestsFixture>
    {
        //todo: rename some view models to match new query/dto names?
        [Test]
        public void Index_WhenGettingLocalAction_ThenShouldRedirectToEmployerPortal()
        {
            Run(f => f.SetCurrentEnvironmentIsLocal(false), f => f.Local(), (f, r) => r.Should().NotBeNull()
                .And.Match<RedirectResult>(a => a.Url == Fix.EmployerPortalUrl));
        }
        
        [Test]
        public Task Index_WhenGettingIndexAction_ThenShouldReturnView()
        {
            return RunAsync(f => f.SetCurrentEnvironmentIsLocal(false), f => f.Index(), (f, r) =>
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
        public HomeController HomeController { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IMapper Mapper { get; set; }
        public Mock<IEnvironment> Environment { get; set; }
        public Mock<IEmployerUrls> EmployerUrls { get; set; }
        public const string EmployerPortalUrl = "https://foo.bar";
        
        public AccountProvidersRouteValues AccountProvidersRouteValues { get; set; }
        public GetAccountProvidersQueryResult GetAccountProvidersQueryResult { get; set; }

        public HomeControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            Mapper = new MapperConfiguration(c => c.AddProfile<ProviderMappings>()).CreateMapper();
            Environment = new Mock<IEnvironment>();
            EmployerUrls = new Mock<IEmployerUrls>();

            EmployerUrls.Setup(au => au.PortalHomepage(null)).Returns(EmployerPortalUrl);
            
            HomeController = new HomeController(Mediator.Object, Mapper, Environment.Object, EmployerUrls.Object);
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

        public HomeControllerTestsFixture SetCurrentEnvironmentIsLocal(bool local)
        {
            Environment.Setup(e => e.IsCurrent(It.IsAny<DasEnv[]>())).Returns(local);
            return this;
        }
    }
}
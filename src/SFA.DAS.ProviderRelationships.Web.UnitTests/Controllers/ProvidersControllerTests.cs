using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.ViewModels;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    public class ProvidersControllerTests : FluentTest<ProvidersControllerTestsFixture>
    {
        [Test]
        public void Search_WhenGettingTheProviderSearchPage_ThenShouldReturnAViewResult()
        {
            Run(f => f.Search(), (f, a) => a.Should().NotBeNull().And.Match<ViewResult>(v =>
                v.ViewName == "" && v.Model.GetType() == typeof(SearchProvidersViewModel)));
        }

        [Test]
        public Task Search_WhenPostingTheProviderSearchPage_ThenShouldReturnARedirectToRouteResult()
        {
            return RunAsync(f => f.SetSearchProvidersQueryResponse(), f => f.PostSearch(), (f, a) => a.Should().NotBeNull().And.Match<RedirectToRouteResult>(r =>
                r.RouteValues["Action"].Equals("Add") && r.RouteValues["ukprn"].Equals(f.SearchProvidersQueryResponse.Ukprn)));
        }
    }

    public class ProvidersControllerTestsFixture
    {
        public ProvidersController ProvidersController { get; set; }
        public SearchProvidersViewModel SearchProvidersViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public SearchProvidersQueryResponse SearchProvidersQueryResponse { get; set; }

        public ProvidersControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            ProvidersController = new ProvidersController(Mediator.Object);
        }

        public ActionResult Search()
        {
            return ProvidersController.Search();
        }

        public Task<ActionResult> PostSearch()
        {
            return ProvidersController.Search(SearchProvidersViewModel);
        }

        public ProvidersControllerTestsFixture SetSearchProvidersQueryResponse()
        {
            SearchProvidersViewModel = new SearchProvidersViewModel
            {
                SearchProvidersQuery = new SearchProvidersQuery
                {
                    Ukprn = "12345678"
                }
            };

            SearchProvidersQueryResponse = new SearchProvidersQueryResponse
            {
                Ukprn = 12345678
            };

            Mediator.Setup(m => m.Send(SearchProvidersViewModel.SearchProvidersQuery, CancellationToken.None)).ReturnsAsync(SearchProvidersQueryResponse);

            return this;
        }
    }
}
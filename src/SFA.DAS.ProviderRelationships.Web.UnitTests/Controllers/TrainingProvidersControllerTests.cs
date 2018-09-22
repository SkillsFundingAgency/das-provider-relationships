using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.ViewModels;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    public class TrainingProvidersControllerTests : FluentTest<TrainingProvidersControllerTestsFixture>
    {
        [Test]
        public void Search_WhenGettingTheTrainingProviderSearchPage_ThenShouldReturnAViewResult()
        {
            Run(f => f.Search(), (f, a) => a.Should().NotBeNull().And.Match<ViewResult>(v =>
                v.ViewName == "" && v.Model.GetType() == typeof(SearchTrainingProvidersViewModel)));
        }

        [Test]
        public Task Search_WhenPostingTheTrainingProviderSearchPage_ThenShouldReturnARedirectToRouteResult()
        {
            return RunAsync(f => f.SetSearchTrainingProvidersQueryResponse(), f => f.PostSearch(), (f, a) => a.Should().NotBeNull().And.Match<RedirectToRouteResult>(r =>
                r.RouteValues["Action"].Equals("Add") && r.RouteValues["ukprn"].Equals(f.SearchTrainingProvidersQueryResponse.TrainingProvider.Ukprn)));
        }
    }

    public class TrainingProvidersControllerTestsFixture
    {
        public TrainingProvidersController TrainingProvidersController { get; set; }
        public SearchTrainingProvidersViewModel SearchTrainingProvidersViewModel { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public SearchTrainingProvidersQueryResponse SearchTrainingProvidersQueryResponse { get; set; }

        public TrainingProvidersControllerTestsFixture()
        {
            Mediator = new Mock<IMediator>();
            TrainingProvidersController = new TrainingProvidersController(Mediator.Object);
        }

        public ActionResult Search()
        {
            return TrainingProvidersController.Search();
        }

        public Task<ActionResult> PostSearch()
        {
            return TrainingProvidersController.Search(SearchTrainingProvidersViewModel);
        }

        public TrainingProvidersControllerTestsFixture SetSearchTrainingProvidersQueryResponse()
        {
            SearchTrainingProvidersViewModel = new SearchTrainingProvidersViewModel
            {
                SearchTrainingProvidersQuery = new SearchTrainingProvidersQuery
                {
                    Ukprn = "12345678"
                }
            };

            SearchTrainingProvidersQueryResponse = new SearchTrainingProvidersQueryResponse
            {
                TrainingProvider = new TrainingProviderDto
                {
                    Ukprn = 12345678
                }
            };

            Mediator.Setup(m => m.Send(SearchTrainingProvidersViewModel.SearchTrainingProvidersQuery, CancellationToken.None)).ReturnsAsync(SearchTrainingProvidersQueryResponse);

            return this;
        }
    }
}
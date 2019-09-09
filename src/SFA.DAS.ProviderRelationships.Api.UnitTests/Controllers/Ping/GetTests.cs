using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.Ping
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public void WhenGettingGetAction_ThenShouldReturnOkResult()
        {
            Run(f => f.Get(), (f, r) => r.Should().NotBeNull().And.BeOfType<OkResult>());
        }
    }

    public class GetTestsFixture
    {
        public PingController Controller { get; set; }

        public GetTestsFixture()
        {
            Controller = new PingController();
        }

        public IHttpActionResult Get()
        {
            return Controller.Get();
        }
    }
}
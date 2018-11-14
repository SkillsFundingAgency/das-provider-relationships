using System.Collections.Specialized;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Environment
{
    [TestFixture]
    [Parallelizable]
    public class EnvironmentTests : FluentTest<EnvironmentTestsFixture>
    {
        #region Current

        [TestCase(DasEnv.LOCAL)]
        [TestCase(DasEnv.AT)]
        [TestCase(DasEnv.MO)]
        [TestCase(DasEnv.DEMO)]
        [TestCase(DasEnv.PROD)]
        [TestCase(DasEnv.TEST)]
        [TestCase(DasEnv.TEST2)]
        public void WhenGettingCurrent_ThenShouldReturnCurrentEnvironment(DasEnv env)
        {
            Run(f => f.SetCurrent(env), f => f.Current(), (f, r) => r.Should().Be(env));
        }        

        #endregion Current

        #region IsCurrent

        [TestCase(false, DasEnv.PROD, new DasEnv[] {})]
        [TestCase(true, DasEnv.MO, new[] {DasEnv.MO})]
        [TestCase(false, DasEnv.MO, new[] {DasEnv.PROD})]
        [TestCase(true, DasEnv.DEMO, new[] {DasEnv.MO, DasEnv.DEMO})]
        [TestCase(false, DasEnv.DEMO, new[] {DasEnv.MO, DasEnv.PROD})]
        public void WhenSuppliedEnvironmentsAreCheckedIfAnyIsTheCurrentEnvironment_TheShouldReturnCorrectIfCurrentStatus(bool expected, DasEnv current, DasEnv[] toCheck)
        {
            Run(f => f.SetCurrent(current), f => f.IsCurrent(toCheck), (f, r) => r.Should().Be(expected));
        }
        
        #endregion IsCurrent
    }

    public class EnvironmentTestsFixture
    {
        public ProviderRelationships.Environment.Environment Environment { get; set; }
        public NameValueCollection AppSettings { get; set; }

        #region Arrange
        
        public EnvironmentTestsFixture()
        {
            AppSettings = new NameValueCollection();
        }

        public void SetCurrent(DasEnv environment)
        {
            AppSettings["EnvironmentName"] = environment.ToString();
            Environment = new ProviderRelationships.Environment.Environment(AppSettings);
        }
        
        #endregion Arrange

        #region Act        
        
        public DasEnv Current()
        {
            return Environment.Current;
        }

        public bool IsCurrent(params DasEnv[] environment)
        {
            return Environment.IsCurrent(environment);
        }
        
        #endregion Act        
    }
}
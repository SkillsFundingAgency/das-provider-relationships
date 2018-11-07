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

        #region Get
        #endregion Get
    }

    public class EnvironmentTestsFixture
    {
        public ProviderRelationships.Environment.Environment Environment { get; set; }
        public NameValueCollection AppSettings { get; set; }
        public DasEnv[] EnvironmentsToCheckIfCurrent { get; set; }

        #region Arrange
        
        public EnvironmentTestsFixture()
        {
            AppSettings = new NameValueCollection();
            Environment = new ProviderRelationships.Environment.Environment(AppSettings);
        }

        public void SetCurrent(DasEnv environment)
        {
            AppSettings["EnvironmentName"] = environment.ToString();
        }

        #endregion Arrange

        #region Act        
        
        public DasEnv Current()
        {
            return Environment.Current;
        }

        public bool IsCurrent()
        {
            return Environment.IsCurrent(EnvironmentsToCheckIfCurrent);
        }
        
        #endregion Act        
    }
}
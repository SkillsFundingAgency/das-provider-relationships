using System.Collections.Specialized;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Environment
{
    [TestFixture]
    [Parallelizable]
    public class EnvironmentTests : FluentTest<EnvironmentTestsFixture>
    {
        [TestCase(DasEnv.LOCAL)]
        [TestCase(DasEnv.AT)]
        [TestCase(DasEnv.TEST)]
        [TestCase(DasEnv.TEST2)]
        [TestCase(DasEnv.PREPROD)]
        [TestCase(DasEnv.PROD)]
        [TestCase(DasEnv.MO)]
        [TestCase(DasEnv.DEMO)]
        public void WhenGettingCurrent_ThenShouldReturnCurrentEnvironment(DasEnv env)
        {
            Run(f => f.SetCurrent(env), f => f.Current(), (f, r) => r.Should().Be(env));
        }        

        [TestCase(false, DasEnv.PROD, new DasEnv[] {})]
        [TestCase(true, DasEnv.MO, new[] {DasEnv.MO})]
        [TestCase(false, DasEnv.MO, new[] {DasEnv.PROD})]
        [TestCase(true, DasEnv.DEMO, new[] {DasEnv.MO, DasEnv.DEMO})]
        [TestCase(false, DasEnv.DEMO, new[] {DasEnv.MO, DasEnv.PROD})]
        public void WhenSuppliedEnvironmentsAreCheckedIfAnyIsTheCurrentEnvironment_TheShouldReturnCorrectIfCurrentStatus(bool expected, DasEnv current, DasEnv[] toCheck)
        {
            Run(f => f.SetCurrent(current), f => f.IsCurrent(toCheck), (f, r) => r.Should().Be(expected));
        }
    }

    public class EnvironmentTestsFixture
    {
        public ProviderRelationships.Environment.Environment Environment { get; set; }
        public NameValueCollection AppSettings { get; set; }
        
        public EnvironmentTestsFixture()
        {
            AppSettings = new NameValueCollection();
        }

        public void SetCurrent(DasEnv environment)
        {
            AppSettings["EnvironmentName"] = environment.ToString();
            Environment = new ProviderRelationships.Environment.Environment(AppSettings);
        }        
        
        public DasEnv Current()
        {
            return Environment.Current;
        }

        public bool IsCurrent(params DasEnv[] environment)
        {
            return Environment.IsCurrent(environment);
        }        
    }
}
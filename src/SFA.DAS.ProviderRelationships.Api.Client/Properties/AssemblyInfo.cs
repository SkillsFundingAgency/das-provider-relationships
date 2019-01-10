using System.Runtime.CompilerServices;

// enable Moq to mock internal types, see... https://stackoverflow.com/questions/17569746/mocking-internal-classes-with-moq-for-unit-testing
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("SFA.DAS.ProviderRelationships")]
[assembly: InternalsVisibleTo("SFA.DAS.ProviderRelationships.UnitTests")]
[assembly: InternalsVisibleTo("SFA.DAS.ProviderRelationships.Api.Client.TestHarness")]
[assembly: InternalsVisibleTo("SFA.DAS.ProviderRelationships.Api.Client.UnitTests")]
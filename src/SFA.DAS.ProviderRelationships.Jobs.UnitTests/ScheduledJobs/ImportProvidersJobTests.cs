using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Jobs.ScheduledJobs;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Jobs.UnitTests.ScheduledJobs;

[TestFixture]
[Parallelizable]
public class ImportProvidersJobTests : FluentTest<ImportProvidersJobTestsFixture>
{
    [Test]
    public Task Run_WhenRunningImportProvidersJob_ThenShouldImportProvidersInBatchesOf1000()
    {
        return TestAsync(f => f.SetProviders(1500), f => f.Run(), f => f.Db.Verify(d => d.ExecuteSqlCommandAsync(
            "EXEC ImportProviders @providers, @now",
            It.Is<SqlParameter>(p => p.ParameterName == "providers"),
            It.Is<SqlParameter>(p => p.ParameterName == "now" && (DateTime)p.Value >= f.Now)), Times.Exactly(2)));
    }
        
    [Test]
    public Task Run_WhenRunningImportProvidersJob_ThenShouldImportProviders()
    {
        return TestAsync(f => f.SetProviders(1500), f => f.Run(), f => f.ImportedProviders.Should().BeEquivalentTo(f.Providers));
    }
}

public class ImportProvidersJobTestsFixture
{
    public DateTime Now { get; set; }
    public Mock<ProviderRelationshipsDbContext> Db { get; set; }
    public ImportProvidersJob Job { get; set; }
    public Mock<IRoatpService> RoatpService { get; set; }
    public List<ProviderRegistration> Providers { get; set; }
    public List<ProviderRegistration> ImportedProviders { get; set; }

    public ImportProvidersJobTestsFixture()
    {
        Now = DateTime.UtcNow;
        Db = new Mock<ProviderRelationshipsDbContext>();
        RoatpService = new Mock<IRoatpService>();
        ImportedProviders = new List<ProviderRegistration>();
            
        Db.Setup(d => d.ExecuteSqlCommandAsync(It.IsAny<string>(), It.IsAny<SqlParameter>(), It.IsAny<SqlParameter>()))
            .Returns(Task.CompletedTask)
            .Callback<string, object[]>((s, p) =>
            {
                var sqlParameter = (SqlParameter)p[0];
                var dataTable = (DataTable)sqlParameter.Value;
                    
                ImportedProviders.AddRange(dataTable.AsEnumerable().Select(r => new ProviderRegistration
                {
                    Ukprn = (int)r[0],
                    ProviderName = (string)r[1]
                }));
            });
            
        Job = new ImportProvidersJob(RoatpService.Object, new Lazy<ProviderRelationshipsDbContext>(() => Db.Object));
    }

    public Task Run()
    {
        return Job.Run(null, null);
    }

    public ImportProvidersJobTestsFixture SetProviders(int count)
    {
        Providers = Enumerable.Range(1, count)
            .Select(i => new ProviderRegistration
            {
                Ukprn = i,
                ProviderName = i.ToString()
            })
            .ToList();
            
        RoatpService.Setup(c => c.GetProviders()).ReturnsAsync(Providers);
            
        return this;
    }
}
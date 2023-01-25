using EfSchemaCompare;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.UnitTests.Models
{
    /// <summary>
    /// https://www.thereformedprogrammer.net/ef-core-taking-full-control-of-the-database-schema/
    /// </summary>
    [TestFixture]
    public class EntityFrameworkSchemaCheckTests
    {
        [Test]
        [Ignore("To be run adhoc (but could live in an integration test)")]
        public void CheckDatabaseSchemaAgainstEntityFrameworkExpectedSchema()
        {
            using (var container = new Container(c =>
            {
                //todo c.AddRegistry<ConfigurationRegistry>();
            }))
            {
                var configuration = container.GetInstance<ProviderRelationshipsConfiguration>();
                
                using (var connection = new SqlConnection(configuration.DatabaseConnectionString))
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseSqlServer(connection);

                    using (var context = new ProviderRelationshipsDbContext(optionsBuilder.Options))
                    {
                        var config = new CompareEfSqlConfig
                        {
                            TablesToIgnoreCommaDelimited = "ClientOutboxData,OutboxData"
                        };
                        
                        config.IgnoreTheseErrors("EXTRA IN DATABASE: SFA.DAS.ProviderRelationships.Database->Column 'Users', column name. Found = Id");
                        
                        var comparer = new CompareEfSql(config);
                        var hasErrors = comparer.CompareEfWithDb(context);

                        hasErrors.Should().BeFalse(comparer.GetAllErrors);
                    }
                }
            }
        }
    }
}
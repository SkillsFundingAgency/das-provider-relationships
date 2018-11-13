using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Data;
using TestSupport.EfSchemeCompare;

namespace SFA.DAS.ProviderRelationships.UnitTests.Models
{
    /// <summary>
    /// https://www.thereformedprogrammer.net/ef-core-taking-full-control-of-the-database-schema/
    /// </summary>
    [TestFixture]
    public class EntityFrameworkSchemaCheckTests
    {
        [Test]
        //[Ignore("To be run adhoc (but could live in an integration test)")]
        public void CheckDatabaseSchemaAgainstEntityFrameworkExpectedSchema()
        {
            //load connection string from config?
            //const string databaseConnectionString = "Server=tcp:phils.database.windows.net,1433;Initial Catalog=SFA.DAS.ProviderRelationships.Database;Persist Security Info=False;User ID=phil;Password=;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            const string databaseConnectionString = "Data Source=(localDB)\\MSSQLLocalDB;Database=SFA.DAS.ProviderRelationships.Database;Integrated Security = true;Trusted_Connection=True;Pooling=False;Connect Timeout=30;MultipleActiveResultSets=True";

            var connection = new SqlConnection(databaseConnectionString);
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseSqlServer(connection);

            using (var context = new ProviderRelationshipsDbContext(optionsBuilder.Options))
            {
                var comparer = new CompareEfSql();
 
                var hasErrors = comparer.CompareEfWithDb(context);

                hasErrors.Should().BeFalse(comparer.GetAllErrors);
            }        
        }
    }
}
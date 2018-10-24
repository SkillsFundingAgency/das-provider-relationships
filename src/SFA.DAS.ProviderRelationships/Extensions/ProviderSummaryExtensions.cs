using System.Collections.Generic;
using System.Data;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class ProviderSummaryExtensions
    {
        public static DataTable ToDataTable(this IEnumerable<ProviderSummary> providers)
        {
            var dataTable = new DataTable();

            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Ukprn", typeof(long)),
                new DataColumn("Name", typeof(string))
            });

            foreach (var provider in providers)
            {
                dataTable.Rows.Add(provider.Ukprn, provider.ProviderName);
            }

            return dataTable;
        }
    }
}
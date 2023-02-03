using System;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsDev(this IConfiguration configuration)
        {
            return configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
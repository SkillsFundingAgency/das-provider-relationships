using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Provider.Shared.UI;
using SFA.DAS.Provider.Shared.UI.Startup;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class NavigationBarSettingsExtensions
    {
        public static IMvcBuilder AddNavigationBarSettings(this IMvcBuilder builder, IConfiguration configuration)
        {
            builder.SetDefaultNavigationSection(NavigationSection.YourCohorts);
            /*
            var featuresConfiguration = configuration
                .GetSection(ProviderRegistrationsConfigurationKeys.FeaturesConfiguration)
                .Get<ProviderFeaturesConfiguration>();
            
            var reservationsToggle =
                featuresConfiguration.FeatureToggles.SingleOrDefault(x =>
                    x.Feature == ProviderFeature.ReservationsWithoutPrefix);

            if (reservationsToggle == null || !reservationsToggle.IsEnabled)
            {
                builder.SuppressNavigationSection(NavigationSection.Reservations);
            }
            */
            return builder;
        }
    }
}

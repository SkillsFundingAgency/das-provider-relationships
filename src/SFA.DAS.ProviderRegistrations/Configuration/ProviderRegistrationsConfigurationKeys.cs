namespace SFA.DAS.ProviderRegistrations.Configuration
{
    public static class ProviderRegistrationsConfigurationKeys
    {
        public const string Encoding = "SFA.DAS.Encoding";

        public const string ProviderRegistrations = "SFA.DAS.ProviderRegistrations";

        public static string AuthenticationSettings = $"{ProviderRegistrations}:AuthenticationSettings";

        //public static string ApprenticeshipInfoServiceConfiguration = $"{ProviderRegistrations}:ApprenticeshipInfoServiceConfiguration";

        //public static string CommitmentsClientApiConfiguration = $"{ProviderRegistrations}:CommitmentsClientApi";

        public static string FeaturesConfiguration = $"{ProviderRegistrations}:Features";
    }
}

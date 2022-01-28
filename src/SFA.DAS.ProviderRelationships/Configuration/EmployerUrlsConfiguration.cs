namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class EmployerUrlsConfiguration : IEmployerUrlsConfiguration
    {
        public string EmployerAccountsBaseUrl { get; set; }
        public string EmployerCommitmentsV2BaseUrl { get; set; }
        public string EmployerFinanceBaseUrl { get; set; }
        public string EmployerPortalBaseUrl { get; set; }
        public string EmployerProjectionsBaseUrl { get; set; }
        public string EmployerRecruitBaseUrl { get; set; }
        public string EmployerUsersBaseUrl { get; set; }
    }
}
namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IEmployerUrlsConfiguration
    {
        string EmployerAccountsBaseUrl { get; }
        string EmployerCommitmentsBaseUrl { get; }
        string EmployerFinanceBaseUrl { get; }
        string EmployerPortalBaseUrl { get; }
        string EmployerProjectionsBaseUrl { get; }
        string EmployerRecruitBaseUrl { get; }
        string EmployerUsersBaseUrl { get; }
    }
}
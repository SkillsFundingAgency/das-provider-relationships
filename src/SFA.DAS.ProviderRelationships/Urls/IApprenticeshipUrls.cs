
namespace SFA.DAS.ProviderRelationships.Urls
{
    //todo: ignored test
    //todo: url use in views
    //todo: password urls
    public interface IApprenticeshipUrls
    {
        string AccountHashedId { get; set; }
        
        string EmployerAccountsAction(string path = null);

        string EmployerAccountsAccountAction(string path = null, string hashedAccountId = null);

        string EmployerCommitmentsAccountAction(string path = null, string hashedAccountId = null);

        string EmployerFinanceAccountAction(string path = null, string hashedAccountId = null);

        string EmployerPortalAccountAction(string path = null, string hashedAccountId = null);

        string EmployerPortalAction(string path = null);

        string EmployerRecruitAccountAction(string path = null, string hashedAccountId = null);
        
        //todo: urls remove path, have method for each url eg. EmployerCommitmentsHomepage (or ManageApprenticeshipsHomepage or whatever) then remove the XAccountAction (apart from intternal helpers)
    }
}
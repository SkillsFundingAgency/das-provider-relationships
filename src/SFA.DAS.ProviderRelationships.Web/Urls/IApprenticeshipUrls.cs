using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    //todo: ignored test
    //todo: url use in views
    //todo: password urls
    public interface IApprenticeshipUrls
    {
        string EmployerAccountsAction(string path = null);

        string EmployerAccountsAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerCommitmentsAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerFinanceAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerPortalAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerPortalAction(string path = null);

        string EmployerRecruitAccountAction(UrlHelper urlHelper, string path = null);
    }
}
namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public interface IProviderUrls
    {
        string Recruit(long ukprn);
        string ProviderManageRecruitEmails(string ukprn);
    }
}
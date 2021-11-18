namespace SFA.DAS.ProviderRelationships.Helpers
{
    public interface IProviderUrls
    {
        string Recruit(string ukprn);
        string ProviderManageRecruitEmails(string ukprn);
    }
}
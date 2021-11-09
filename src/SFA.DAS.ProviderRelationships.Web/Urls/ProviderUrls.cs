using System;
using System.Net;

namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public class ProviderUrls : IProviderUrls
    {
        private readonly IEnvironmentService _environmentService;
        private const ProviderRecruitBaseUrlFormat = "https://recruit.{0}pas.apprenticeships.education.gov.uk";

        public ProviderUrls(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }
        
        public string ProviderManageRecruitEmails(string ukprn) => Recruit("notifications-manage/", ukprn);
        public string Recruit(string ukprn) => Recruit(null, ukprn);
        
        private string Recruit(string path, string ukprn) => Action(ProviderRecruitBaseUrlFormat, path, accountHashedId);
        

        private string Action(string baseUrl, string path)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("Value cannot be null or white space", nameof(baseUrl));
            }
            
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}
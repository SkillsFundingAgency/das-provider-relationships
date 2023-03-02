using System.Net;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi
{
    public class GetEmployerAccountRequest : IGetApiRequest
    {
        private readonly string _userId;
        private readonly string _email;

        public GetEmployerAccountRequest(string userId, string email)
        {
            _userId = userId;
            _email = WebUtility.UrlEncode(email);
        }

        public string GetUrl => $"accountusers/{_userId}/accounts?email={_email}";
    }
}
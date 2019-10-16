using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery
{
    public class GetUnsubscribedQuery : IRequest<bool>
    {
        public GetUnsubscribedQuery(string ukprn, string emailAddress)
        {
            Ukprn = ukprn;
            EmailAddress = emailAddress;
        }

        public string Ukprn { get; }

        public string EmailAddress { get; }
    }
}
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery
{
    public class GetUnsubscribedQuery : IRequest<bool>
    {
        public GetUnsubscribedQuery(long ukprn, string emailAddress)
        {
            Ukprn = ukprn;
            EmailAddress = emailAddress;
        }

        public long Ukprn { get; }

        public string EmailAddress { get; }
    }
}
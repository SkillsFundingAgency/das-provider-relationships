namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery
{
    public class GetUnsubscribedQueryResult
    {
        public GetUnsubscribedQueryResult(bool unsubscribed)
        {
            Unsubscribed = unsubscribed;
        }

        public bool Unsubscribed { get; }
    }
}
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery
{
    public class GetInvitationQuery : IRequest<GetInvitationQueryResult>
    {
        public GetInvitationQuery(string ukprn, string userRef, string sortColumn, string sortDirection)
        {
            Ukprn = ukprn;
            UserRef = userRef;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }

        public string Ukprn { get; }

        public string UserRef { get; }

        public string SortColumn { get; }

        public string SortDirection { get; }
    }
}
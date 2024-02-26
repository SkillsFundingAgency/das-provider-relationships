namespace SFA.DAS.ProviderRelationships.Api.Models.Requests
{
    public class AddAccountProviderFromInvitationPostRequest
    {
        public Guid CorrelationId { get; set; }
        public Guid UserRef { get; set; }
    }
}

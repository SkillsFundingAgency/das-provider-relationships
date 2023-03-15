namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class InvitationAccountProviderRouteValues
    {
        [Required]
        public string AccountHashedId { get; set; }

        [Required]
        public Guid? CorrelationId { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
    }
}
namespace SFA.DAS.ProviderRelationships.Models
{
    // we currently need this CLR class for the join
    // see, https://github.com/aspnet/EntityFrameworkCore/issues/1368
    public class AccountLegalEntityProvider
    {
        public virtual long AccountLegalEntityId { get; set; }
        public virtual long Ukprn { get; set; }
        
        public virtual AccountLegalEntity AccountLegalEntity { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
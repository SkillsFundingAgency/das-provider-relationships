namespace SFA.DAS.ProviderRelationships.Models
{
    // we currently need this CLR class for the join
    // see, https://github.com/aspnet/EntityFrameworkCore/issues/1368
    public class AccountLegalEntityProvider
    {
        public virtual long AccountLegalEntityId { get; protected set; }
        public virtual long Ukprn { get; protected set; }
        
        public virtual AccountLegalEntity AccountLegalEntity { get; protected set; }
        public virtual Provider Provider { get; protected set; }
        
//        public AccountLegalEntityProvider(long accountLegalEntityId, long ukprn)
//        {
//            AccountLegalEntityId = accountLegalEntityId;
//            Ukprn = ukprn;
//        }

        public AccountLegalEntityProvider(long accountLegalEntityId, Provider provider)
        {
            AccountLegalEntityId = accountLegalEntityId;
            Ukprn = provider.Ukprn;
            Provider = provider;
        }
        
        protected AccountLegalEntityProvider()
        {
        }
    }
}
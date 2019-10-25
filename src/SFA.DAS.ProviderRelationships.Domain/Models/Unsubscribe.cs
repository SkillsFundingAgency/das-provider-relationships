using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class Unsubscribe : Entity
    {
        public string EmailAddress { get; private set; }

        public long Ukprn { get; private set; }

        public Unsubscribe(string emailAddress, long ukprn)
        {
            EmailAddress = emailAddress;
            Ukprn = ukprn;
        }

        private Unsubscribe()
        {
        }
    }
}

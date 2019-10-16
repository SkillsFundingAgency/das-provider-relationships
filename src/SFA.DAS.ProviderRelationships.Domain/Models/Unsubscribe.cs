using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class Unsubscribe : Entity
    {
        public string EmailAddress { get; private set; }

        public string Ukprn { get; private set; }

        public Unsubscribe(string emailAddress, string ukprn)
        {
            EmailAddress = emailAddress;
            Ukprn = ukprn;
        }

        private Unsubscribe()
        {
        }
    }
}

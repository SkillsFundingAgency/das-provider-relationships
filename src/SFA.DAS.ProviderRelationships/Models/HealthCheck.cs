using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class HealthCheck : Entity
    {
        public int Id { get; private set; }
        public User User { get; private set; }
        public Guid UserRef { get; private set; }
        public DateTime SentApprenticeshipInfoServiceApiRequest { get; private set; }
        public DateTime? ReceivedApprenticeshipInfoServiceApiResponse { get; private set; }
        public DateTime SentProviderRelationshipsApiRequest { get; private set; }
        public DateTime? ReceivedProviderRelationshipsApiResponse { get; private set; }
        public DateTime PublishedProviderRelationshipsEvent { get; private set; }
        public DateTime? ReceivedProviderRelationshipsEvent { get; private set; }

        public HealthCheck(User user)
        {
            User = user;
            UserRef = user.Ref;
        }

        private HealthCheck()
        {
        }
        
        public async Task Run(Func<Task> apprenticeshipInfoServiceApiRequest, Func<Task> providerRelationshipsApiRequest)
        {
            await SendApprenticeshipInfoServiceApiRequest(apprenticeshipInfoServiceApiRequest);
            await SendProviderRelationshipsApiRequest(providerRelationshipsApiRequest);
            PublishProviderRelationshipsEvent();
        }

        public void ReceiveProviderRelationshipsEvent()
        {
            ReceivedProviderRelationshipsEvent = DateTime.UtcNow;
        }

        private async Task SendApprenticeshipInfoServiceApiRequest(Func<Task> run)
        {
            SentApprenticeshipInfoServiceApiRequest = DateTime.UtcNow;

            try
            {
                await run();
                ReceivedApprenticeshipInfoServiceApiResponse = DateTime.UtcNow;
            }
            catch (Exception)
            {
            }
        }

        private async Task SendProviderRelationshipsApiRequest(Func<Task> run)
        {
            SentProviderRelationshipsApiRequest = DateTime.UtcNow;

            try
            {
                await run();
                ReceivedProviderRelationshipsApiResponse = DateTime.UtcNow;
            }
            catch (Exception)
            {
            }
        }

        private void PublishProviderRelationshipsEvent()
        {
            PublishedProviderRelationshipsEvent = DateTime.UtcNow;

            Publish(() => new HealthCheckEvent(Id, PublishedProviderRelationshipsEvent));
        }
    }
}
using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class HealthCheck : Entity
    {
        public virtual int Id { get; protected set; }
        public virtual User User { get; protected set; }
        public virtual Guid UserRef { get; protected set; }
        public virtual DateTime SentApprenticeshipInfoServiceApiRequest { get; protected set; }
        public virtual DateTime? ReceivedApprenticeshipInfoServiceApiResponse { get; protected set; }
        public virtual DateTime SentProviderRelationshipsApiRequest { get; protected set; }
        public virtual DateTime? ReceivedProviderRelationshipsApiResponse { get; protected set; }
        public virtual DateTime PublishedProviderRelationshipsEvent { get; protected set; }
        public virtual DateTime? ReceivedProviderRelationshipsEvent { get; protected set; }

        public HealthCheck(User user)
        {
            User = user;
            UserRef = user.Ref;
        }

        protected HealthCheck()
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
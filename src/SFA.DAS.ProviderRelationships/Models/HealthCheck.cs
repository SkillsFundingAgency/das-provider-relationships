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
        public virtual DateTime PublishedProviderRelationshipsEvent { get; protected set; }
        public virtual DateTime? ReceivedProviderRelationshipsEvent { get; protected set; }

        public HealthCheck(User user)
        {
            User = user;
        }

        protected HealthCheck()
        {
        }
        
        public async Task Run(Func<Task> apprenticeshipInfoServiceApiRequest)
        {
            await SendApprenticehipInfoServiceApiRequest(apprenticeshipInfoServiceApiRequest);
            PublishProviderRelationshipsEvent();
        }

        public void ReceiveProviderRelationshipsEvent(HealthCheckEvent message)
        {
            ReceivedProviderRelationshipsEvent = DateTime.UtcNow;
        }

        private async Task SendApprenticehipInfoServiceApiRequest(Func<Task> run)
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

        private void PublishProviderRelationshipsEvent()
        {
            PublishedProviderRelationshipsEvent = DateTime.UtcNow;

            Publish(() => new HealthCheckEvent(Id, PublishedProviderRelationshipsEvent));
        }
    }
}
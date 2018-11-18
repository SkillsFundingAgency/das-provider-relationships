using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class ReceiveProviderRelationshipsHealthCheckEventCommand : IRequest
    {
        public int Id { get; }

        public ReceiveProviderRelationshipsHealthCheckEventCommand(int id)
        {
            Id = id;
        }
    }
}
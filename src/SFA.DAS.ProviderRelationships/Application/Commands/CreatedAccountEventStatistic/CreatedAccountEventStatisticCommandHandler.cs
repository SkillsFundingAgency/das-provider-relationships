using MediatR;
using SFA.DAS.ProviderRelationships.Slack;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreatedAccountEventStatistic
{
    public class CreatedAccountEventStatisticCommandHandler : RequestHandler<CreatedAccountEventStatisticCommand>
    {
        private readonly ISlackClient _slackClient;

        public CreatedAccountEventStatisticCommandHandler(ISlackClient slackClient)
        {
            _slackClient = slackClient;
        }

        protected override void Handle(CreatedAccountEventStatisticCommand request)
        {
            _slackClient.PostCreatedEmployerAccount(request.Name);
        }
    }
}
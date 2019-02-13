using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedPermissionsEventNotifyCommandHandlerTests : FluentTest<UpdatedPermissionsEventNotifyCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingUpdatedPermissionsEventNotifyCommand_ThenShouldCallClientToNotify()
        {
            return RunAsync(f => f.Handle(),
                f => f.Client.Verify(c => c.SendEmailToAllProviderRecipients(f.AccountProviderId,
                    It.Is<ProviderEmailRequest>(r =>
                        r.TemplateId == "UpdatedPermissionsEventNotification" &&
                        r.Tokens["GrantedOperations"] == f.ExpectedOperationsString &&
                        r.Tokens["Ukprn"] == f.Ukprn.ToString()
                        ))));
        }
    }

    public class UpdatedPermissionsEventNotifyCommandHandlerTestsFixture
    {
        public UpdatedPermissionsEventNotifyCommand Command { get; set; }
        public IRequestHandler<UpdatedPermissionsEventNotifyCommand, Unit> Handler { get; set; }
        public Mock<IPasAccountApiClient> Client { get; set; }
        public long AccountProviderId { get; set; }
        public long Ukprn { get; set; }
        public HashSet<Operation> GrantedOperations { get; set; }
        public string ExpectedOperationsString { get; set; }

        public UpdatedPermissionsEventNotifyCommandHandlerTestsFixture()
        {
            AccountProviderId = 112;
            Ukprn = 114;
            GrantedOperations = new HashSet<Operation>{ Operation.CreateCohort };
            ExpectedOperationsString = "CreateCohort";

            Command = new UpdatedPermissionsEventNotifyCommand(AccountProviderId, Ukprn, GrantedOperations);
            Client = new Mock<IPasAccountApiClient>();
            Handler = new UpdatedPermissionsEventNotifyCommandHandler(Client.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
        }

        
    }
}
using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.AddedPayeScheme
{
    public class AddedPayeSchemeCommand : IRequest
    {
        public long AccountId { get; set; }

        public string UserName { get; set; }

        public Guid UserRef { get; set; }

        public string PayeRef { get; set; }

        public string Aorn { get; set; }

        public string SchemeName { get; set; }

        public string CorrelationId { get; set; }

        public AddedPayeSchemeCommand(long accountId, string userName, Guid userRef, string payeRef, string aorn, string schemeName, string correlationId)
        {
            AccountId = accountId;
            UserName = userName;
            UserRef = userRef;
            PayeRef = payeRef;
            Aorn = aorn;
            SchemeName = schemeName;
            CorrelationId = correlationId;
        }
    }
}
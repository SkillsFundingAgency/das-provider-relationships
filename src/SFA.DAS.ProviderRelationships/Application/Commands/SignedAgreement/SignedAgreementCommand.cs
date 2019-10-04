using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SignedAgreement
{
    public class SignedAgreementCommand : IRequest
    {
        public long AccountId { get; set; }

        public string OrganisationName { get; protected set; }

        public long AgreementId { get; protected set; }

        public long LegalEntityId { get; protected set; }

        public bool CohortCreated { get; protected set; }

        public string UserName { get; set; }

        public Guid UserRef { get; set; }

        public Guid CorrelationId { get; set; }

        public SignedAgreementCommand(long accountId, long agreementId, string organisationName, long legalEntityId, bool cohortCreated, string userName, Guid userRef, Guid correlationId)
        {
            AccountId = accountId;
            AgreementId = agreementId;
            OrganisationName = organisationName;
            UserName = userName;
            UserRef = userRef;
            CohortCreated = cohortCreated;
            LegalEntityId = legalEntityId;
            CorrelationId = correlationId;
        }
    }
}
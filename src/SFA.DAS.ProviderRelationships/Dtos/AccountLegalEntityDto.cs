using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class AccountLegalEntityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
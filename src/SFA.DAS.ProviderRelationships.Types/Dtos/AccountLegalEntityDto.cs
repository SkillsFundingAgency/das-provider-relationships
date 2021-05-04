using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class AccountLegalEntityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool HadPermissions { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
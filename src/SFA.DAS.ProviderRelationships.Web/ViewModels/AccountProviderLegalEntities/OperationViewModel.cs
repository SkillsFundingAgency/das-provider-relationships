using System;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    [Serializable]
    public class OperationViewModel
    {
        public bool IsEnabled { get; set; }
        public Operation Value { get; set; }
    }
}
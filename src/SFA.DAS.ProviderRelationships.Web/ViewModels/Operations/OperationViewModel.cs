using System;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Operations
{
    [Serializable]
    public class OperationViewModel
    {
        public bool? IsEnabled { get; set; }
        public Operation Value { get; set; }

        public override bool Equals(object obj)
        {
            var operationViewModel = obj as OperationViewModel;

            return Equals(operationViewModel);
        }
        public override int GetHashCode()
        {
            return ((short)Value).GetHashCode();
        }

        public bool Equals(OperationViewModel operationtoCompare)
        {
            if (operationtoCompare == null) return false;
            return Value.Equals(operationtoCompare.Value);
        }
    }
}
using System;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions
{
    [Serializable]
    public class PermissionViewModel
    {
        public State State { get; set; }
        public Permission Value { get; set; }

        public override bool Equals(object obj)
        {
            var operationViewModel = obj as PermissionViewModel;

            return Equals(operationViewModel);
        }

        public override int GetHashCode()
        {
            return ((short)Value).GetHashCode();
        }
    }
}
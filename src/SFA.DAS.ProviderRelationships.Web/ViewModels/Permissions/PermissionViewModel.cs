using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions
{
    public class PermissionViewModel
    {
        public State State { get; set; }
        public Permission Value { get; set; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PermissionViewModel compare)) return false;
            return State.Equals(compare.State) && Value.Equals(compare.Value);
        }
    }
}
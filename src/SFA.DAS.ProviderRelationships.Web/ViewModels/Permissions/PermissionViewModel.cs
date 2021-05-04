using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions
{
    public class PermissionViewModel
    {
        public State? State { get; set; }

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

        public override string ToString()
        {
            switch (Value)
            {
                case Permission.CreateCohort:
                {
                    switch (State)
                    {
                        case Types.Models.State.Yes: return "Add apprentice records";
                        case Types.Models.State.No: return "Cannot add apprentice records";
                        default: return string.Empty;
                    }
                }
                case Permission.Recruitment:
                {
                    switch (State)
                    {
                        case Types.Models.State.Yes: return "Create and publish job adverts";
                        case Types.Models.State.No: return "Cannot create job adverts";
                        case Types.Models.State.Conditional: return "Create job adverts";
                        default: return string.Empty;
                    }
                }

                default: return string.Empty;
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class PermissionOperationExtensions
    {
        public static string GetDescription(this Permission permission)
        {
            return permission.GetType()
                .GetMember(permission.ToString()).First()
                .GetCustomAttributes<DisplayAttribute>().First()
                .Name;
        }

        public static List<PermissionViewModel> ToPermissions(this IList<Operation> operations)
        {
            return Enum.GetValues(typeof(Permission))
                .Cast<Permission>()
                .Select(p => new PermissionViewModel {
                    Value = p,
                    State = p.Map(operations)
                }).ToList();
        }

        public static HashSet<Operation> ToOperations(this IList<PermissionViewModel> permissions)
        {
            return permissions.SelectMany(p => p.Map()).ToHashSet();
        }

        private static State Map(this Permission permission, IList<Operation> operations)
        {
            switch (permission)
            {
                case Permission.CreateCohort: return operations.Contains(Operation.CreateCohort) ? State.Yes : State.No;

                case Permission.Recruitment:
                {
                    if (operations.Contains(Operation.Recruitment) && operations.Contains(Operation.RecruitmentRequiresReview))
                    {
                        return State.Conditional;
                    }
                    
                    if (operations.Contains(Operation.Recruitment))
                    {
                        return State.Yes;
                    }
                    
                    return State.No;
                }
            }

            return State.No;
        }

        private static IEnumerable<Operation> Map(this PermissionViewModel p)
        {
            if (p.State != State.No)
            {
                switch (p.Value)
                {
                    case Permission.CreateCohort: return new List<Operation> {Operation.CreateCohort};

                    case Permission.Recruitment:
                    {
                        return p.State == State.Conditional
                            ? new List<Operation> {Operation.Recruitment, Operation.RecruitmentRequiresReview}
                            : new List<Operation> {Operation.Recruitment};
                    }
                }
            }

            return new List<Operation>();
        }

        public static string Status(this PermissionViewModel permissionViewModel)
        {
            switch (permissionViewModel.State)
            {
                case State.No: return "Do not allow";
                case State.Yes: return "Allow";
                case State.Conditional:
                {
                    switch (permissionViewModel.Value)
                    {
                        case Permission.Recruitment: return "Allow, but I want to review adverts before they�re advertised";
                        default: return string.Empty;
                    }
                }

                default: return string.Empty;
            }
        }
    }
}
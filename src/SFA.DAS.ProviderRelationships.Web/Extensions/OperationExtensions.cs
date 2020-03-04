using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class OperationExtensions
    {
        public static string GetDescription(this Operation operation)
        {
            return operation.GetType()
                .GetMember(operation.ToString()).First()
                .GetCustomAttributes<DisplayAttribute>().First()
                .Name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class OperationExtensions
    {
        private static readonly Dictionary<Operation, string> OperationDescriptions = Enum.GetValues(typeof(Operation))
            .Cast<Operation>()
            .ToDictionary(v => v, v => Regex.Replace(v.ToString(), "(.)([A-Z])", m => $"{m.Groups[1].Value} {m.Groups[2].Value.ToLower()}"));
        
        public static string GetDescription(this Operation operation)
        {
            return OperationDescriptions[operation];
        }
    }
}
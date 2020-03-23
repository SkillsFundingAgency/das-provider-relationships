using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
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

        public static MvcHtmlString GetDescriptionPartial(this Operation operation, HtmlHelper html)
        {
            switch (operation)
            {
                case Operation.Recruitment:
                    return System.Web.Mvc.Html.PartialExtensions.Partial(html, "_OperationRecruitmentDescription");
                default:
                    return System.Web.Mvc.Html.PartialExtensions.Partial(html, "_OperationDefaultDescription");
            }
        } 

        public static Operation Previous(this List<Operation> operations, Operation current)
        {
            var previous = Operation.NotSet;
            var currentOperationId = (short)current;

            foreach (var operation in operations)
            {
                var operationId = (short)operation;
                if (operationId < currentOperationId)
                {
                    previous = operation;
                }
            }

            return previous;
        }

        public static Operation Next(this List<Operation> operations, Operation current)
        { 
            if (current == Operation.NotSet && operations.Any())
            {
                return operations.First();
            }

            var currentOperationId = (short)current;

            foreach (var operation in operations)
            {
                var operationId = (short)operation;
                if (operationId > currentOperationId)
                {
                    return operation;
                }
            }

            return Operation.NotSet;
        }

        public static bool IsLast(this List<Operation> operations, Operation current)
        {
            return (short)current == (short)Enum.GetValues(typeof(Operation)).Cast<Operation>().Max();
        }

        public static Operation Last(this List<Operation> operations)        
        {
            var last = Operation.NotSet;

            foreach (var operation in operations)
            {
                var operationId = (short)operation;
                if (operationId > (short)last)
                {
                    last = operation;
                }
            }

            return last;
        }
    }
}
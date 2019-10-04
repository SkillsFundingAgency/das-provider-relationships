using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SFA.DAS.ProviderRegistrations.Extensions
{
    public static class EnumerableExpressionHelper
    {
        public static Expression<Func<TSource, String>> CreateEnumToStringExpression<TSource, TMember>(
            Expression<Func<TSource, TMember>> memberAccess, string defaultValue = "")
        {
            var type = typeof(TMember);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("TMember must be an Enum type");
            }

            var enumValues = (TMember[]) Enum.GetValues(type);
            var enumNames = (from object value in Enum.GetValues(type)
                select value.GetType()
                    .GetMember(value.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .GetName()).ToArray();

            var inner = (Expression)Expression.Constant(defaultValue);

            var parameter = memberAccess.Parameters[0];

            for (int i = 0; i < enumValues.Length; i++)
            {
                inner = Expression.Condition(
                    Expression.Equal(memberAccess.Body, Expression.Constant(enumValues[i])),
                    Expression.Constant(enumNames[i]),
                    inner);
            }

            var expression = Expression.Lambda<Func<TSource, String>>(inner, parameter);

            return expression;
        }
    }
}
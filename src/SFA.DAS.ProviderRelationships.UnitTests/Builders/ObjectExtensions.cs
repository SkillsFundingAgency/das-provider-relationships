using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public static class PropertySetterExtension
    {
        public static TObject SetPropertyTo<TObject, TProperty>(this TObject obj, Expression<Func<TObject, TProperty>> property, TProperty value)
        {
            var memberExp = (MemberExpression)property.Body;

            ((PropertyInfo)memberExp.Member).SetValue(obj, value);

            return obj;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Builders
{
    public static class EntityExtensions
    {
        public static TEntity Set<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, TProperty value) where TEntity : Entity
        {
            var memberExpression = (MemberExpression) property.Body;
            var propertyInfo = (PropertyInfo) memberExpression.Member;

            propertyInfo.SetValue(entity, value);

            return entity;
        }
        
        public static TEntity Add<TEntity, TProperty, TItem>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, TItem item) where TEntity : Entity where TProperty : IEnumerable<TItem>
        {
            var memberExpression = (MemberExpression) property.Body;
            var propertyInfo = (PropertyInfo) memberExpression.Member;
            var collection = (ICollection<TItem>) propertyInfo.GetValue(entity);

            collection.Add(item);
            
            return entity;
        }
        
        public static TEntity AddRange<TEntity, TProperty, TItem>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, IEnumerable<TItem> items) where TEntity : Entity where TProperty : IEnumerable<TItem>
        {
            var memberExpression = (MemberExpression) property.Body;
            var propertyInfo = (PropertyInfo) memberExpression.Member;
            var collection = (ICollection<TItem>) propertyInfo.GetValue(entity);

            foreach (var item in items)
            {
                collection.Add(item);
            }
            
            return entity;
        }
    }
}
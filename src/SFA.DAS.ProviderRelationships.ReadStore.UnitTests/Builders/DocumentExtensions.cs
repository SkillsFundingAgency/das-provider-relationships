using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders
{
    public static class DocumentExtensions
    {
        internal static TDocument Set<TDocument, TProperty>(this TDocument document, Expression<Func<TDocument, TProperty>> property, TProperty value) where TDocument : Document
        {
            var memberExpression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            
            propertyInfo.SetValue(document, value);

            return document;
        }
        
        internal static TDocument Add<TDocument, TProperty, TItem>(this TDocument entity, Expression<Func<TDocument, TProperty>> property, TItem item) where TDocument : Document where TProperty : IEnumerable<TItem>
        {
            var memberExpression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var collection = (ICollection<TItem>)propertyInfo.GetValue(entity);

            collection.Add(item);
            
            return entity;
        }
        
        internal static TDocument AddRange<TDocument, TProperty, TItem>(this TDocument entity, Expression<Func<TDocument, TProperty>> property, IEnumerable<TItem> items) where TDocument : Document where TProperty : IEnumerable<TItem>
        {
            var memberExpression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var collection = (ICollection<TItem>)propertyInfo.GetValue(entity);

            foreach (var item in items)
            {
                collection.Add(item);
            }
            
            return entity;
        }
    }
}
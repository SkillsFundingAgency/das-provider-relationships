using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public static class DocumentExtensions
    {
        public static TDocument Set<TDocument, TProperty>(this TDocument document, Expression<Func<TDocument, TProperty>> property, TProperty value) where TDocument : IDocument
        {
            var memberExpression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            
            propertyInfo.SetValue(document, value);

            return document;
        }
        
        public static TDocument Add<TDocument, TProperty, TItem>(this TDocument entity, Expression<Func<TDocument, TProperty>> property, TItem item) where TDocument : IDocument where TProperty : IEnumerable<TItem>
        {
            var memberExpression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var collection = (ICollection<TItem>)propertyInfo.GetValue(entity);

            collection.Add(item);
            
            return entity;
        }
        
        public static TDocument AddRange<TDocument, TProperty, TItem>(this TDocument entity, Expression<Func<TDocument, TProperty>> property, IEnumerable<TItem> items) where TDocument : IDocument where TProperty : IEnumerable<TItem>
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
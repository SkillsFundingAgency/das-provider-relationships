﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests
{
    public static class CosmosDbExtensions
    {

        public static ResourceResponse<T> ToResourceResponse<T>(this T resource, HttpStatusCode statusCode,
            IDictionary<string, string> responseHeaders = null) where T : Resource, new()
        {
            var resourceResponse = new ResourceResponse<T>(resource);
            var documentServiceResponseType =
                Type.GetType(
                    "Microsoft.Azure.Documents.DocumentServiceResponse, Microsoft.Azure.DocumentDB.Core, Version=2.1.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var headers = new NameValueCollection {{"x-ms-request-charge", "0"}};

            if (responseHeaders != null)
            {
                foreach (var responseHeader in responseHeaders)
                {
                    headers[responseHeader.Key] = responseHeader.Value;
                }
            }

            var headersDictionaryType =
                Type.GetType(
                    "Microsoft.Azure.Documents.Collections.DictionaryNameValueCollection, Microsoft.Azure.DocumentDB.Core, Version=2.1.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            var headersDictionaryInstance = Activator.CreateInstance(headersDictionaryType, headers);

            var arguments = new[] {Stream.Null, headersDictionaryInstance, statusCode, null};

            var documentServiceResponse = documentServiceResponseType.GetTypeInfo().GetConstructors(flags)[0]
                .Invoke(arguments);

            var responseField = typeof(ResourceResponse<T>).GetTypeInfo().GetField("response", flags);

            responseField?.SetValue(resourceResponse, documentServiceResponse);

            return resourceResponse;
        }


        public static FeedResponse<T> ToFeedResponse<T>(this IQueryable<T> resource,
            IDictionary<string, string> responseHeaders = null)
        {
            var feedResponseType =
                Type.GetType(
                    "Microsoft.Azure.Documents.Client.FeedResponse`1, Microsoft.Azure.DocumentDB.Core, Version=2.1.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var headers = new NameValueCollection {
                {"x-ms-request-charge", "0"},
                {"x-ms-activity-id", Guid.NewGuid().ToString()}
            };

            if (responseHeaders != null)
            {
                foreach (var responseHeader in responseHeaders)
                {
                    headers[responseHeader.Key] = responseHeader.Value;
                }
            }

            var headersDictionaryType =
                Type.GetType(
                    "Microsoft.Azure.Documents.Collections.DictionaryNameValueCollection, Microsoft.Azure.DocumentDB.Core, Version=2.1.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            var headersDictionaryInstance = Activator.CreateInstance(headersDictionaryType, headers);

            var arguments = new []
                {resource, resource.Count(), headersDictionaryInstance, false, null, null, null, 0};

            if (feedResponseType != null)
            {
                var t = feedResponseType.MakeGenericType(typeof(T));

                var constructorInfo = t.GetTypeInfo().GetConstructors(flags);
                var feedResponse = constructorInfo[0].Invoke(arguments);

                return (FeedResponse<T>) feedResponse;
            }

            return new FeedResponse<T>();
        }

    }
}

using System;
using System.Net;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class StringExtensions
    {
        public static string HtmlDecode(this string input)
        {
            var output = WebUtility.HtmlDecode(input);

            return output;
        }

        public static string HtmlEncode(this string input)
        {
            var output = WebUtility.HtmlEncode(input);

            return output;
        }
        
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
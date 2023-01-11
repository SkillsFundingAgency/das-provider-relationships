﻿using System;
using System.ComponentModel;
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
        
        //https://stackoverflow.com/a/4878276/1123275
        public static T? ToNullable<T>(this string s) where T : struct
        {
            if (string.IsNullOrWhiteSpace(s))
                return default;

            TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
            return (T)conv.ConvertFrom(s)!;
        }
    }
}
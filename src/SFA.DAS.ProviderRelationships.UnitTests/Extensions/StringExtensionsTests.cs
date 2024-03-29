﻿using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.UnitTests.Extensions
{
    [TestFixture]
    public class IntNullableTests : NullableTests<int> { }
    
    [TestFixture]
    public class FloatNullableTests : NullableTests<float> { }
    
    [TestFixture]
    public class BoolNullableTests : NullableTests<bool> { }
    
    [TestFixture]
    public class DateTimeNullableTests : NullableTests<char> { }

    public abstract class NullableTests<T> where T : struct
    {
        [Test]
        public void When_Calling_ToNullable_And_Null_Then_Returns_Null()
        {
            string source = null;
            var result = source.ToNullable<T>();
            result.HasValue.Should().BeFalse();
            result.Should().BeNull();
        }
        
        [Test, AutoData]
        public void When_Calling_ToNullable_And_Has_Value_Then_Converts_To_Type(T source)
        {
            // note: this implicit cast to string of DateTime strips milliseconds which then results in inequality
            var sourceString = $"{source}"; 
            var result = sourceString.ToNullable<T>();
            result.Should().BeOfType(typeof(T));
            result.HasValue.Should().BeTrue();
            result.Should().Be(source);
        }
    }
}
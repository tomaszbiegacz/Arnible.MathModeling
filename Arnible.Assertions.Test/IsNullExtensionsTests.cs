using System;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsNullExtensionsTests
    {
        [Fact]
        public void Value_Error()
        {
            int? value = 2;
            Assert.Throws<AssertException>(() => value.AssertIsNull());
        }
        
        [Fact]
        public void Value_Ok()
        {
            int? value = null;
            value.AssertIsNull();
        }
        
        [Fact]
        public void Reference_Error()
        {
            Type? value = typeof(IsNullExtensionsTests);
            Assert.Throws<AssertException>(() => value.AssertIsNull());
        }
        
        [Fact]
        public void Reference_Ok()
        {
            Type? value = null;
            value.AssertIsNull();
        }
    }
}
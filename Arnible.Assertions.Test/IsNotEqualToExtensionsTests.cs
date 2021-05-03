using System;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsNotEqualToExtensionsTests
    {
        [Fact]
        public void Value_Error()
        {
            Assert.Throws<AssertException>(() => 2.AssertIsNotEqualTo(2));
        }
        
        [Fact]
        public void Value_Ok()
        {
            3.AssertIsNotEqualTo(2);
        }

        enum SomeEnum
        {
            a = 1,
            b
        }
        
        [Fact]
        public void Enum_Error()
        {
            Assert.Throws<AssertException>(() => SomeEnum.a.AssertIsNotEqualToEnum(SomeEnum.a));
        }
        
        [Fact]
        public void Enum_Ok()
        {
            SomeEnum.a.AssertIsNotEqualToEnum(SomeEnum.b);
        }
    }
}
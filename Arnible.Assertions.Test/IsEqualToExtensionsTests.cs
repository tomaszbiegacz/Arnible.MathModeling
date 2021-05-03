using System;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsEqualToExtensionsTests
    {
        [Fact]
        public void Value_Error()
        {
            Assert.Throws<AssertException>(() => 2.AssertIsEqualTo(3));
        }
        
        [Fact]
        public void Value_Ok()
        {
            2.AssertIsEqualTo(2);
        }

        enum SomeEnum
        {
            a = 1,
            b
        }
        
        [Fact]
        public void Enum_Error()
        {
            Assert.Throws<AssertException>(() => SomeEnum.a.AssertIsEqualToEnum(SomeEnum.b));
        }
        
        [Fact]
        public void Enum_Ok()
        {
            SomeEnum.a.AssertIsEqualToEnum(SomeEnum.a);
        }
        
        [Fact]
        public void NullableValue_Error()
        {
            decimal? actual = 2;
            decimal expected = 3;
            try
            {
                actual.AssertIsEqualTo(in expected);
                throw new Exception("I should not get here");
            }
            catch (AssertException)
            {
                // all is ok
            }
        }
        
        [Fact]
        public void NullableValue_Null()
        {
            decimal? actual = null;
            decimal expected = 3;
            try
            {
                actual.AssertIsEqualTo(in expected);
                throw new Exception("I should not get here");
            }
            catch (AssertException)
            {
                // all is ok
            }
        }
        
        [Fact]
        public void NullableValue_Ok()
        {
            decimal? actual = 2;
            decimal expected = 2;
            actual.AssertIsEqualTo(in expected);
        }
        
        [Fact]
        public void ushort_Error()
        {
            ushort actual = 1;
            int expected = 2;
            Assert.Throws<AssertException>(() => actual.AssertIsEqualTo(expected));
        }
        
        [Fact]
        public void ushort_Ok()
        {
            ushort actual = 1;
            int expected = 1;
            actual.AssertIsEqualTo(expected);
        }
        
        [Fact]
        public void byte_Error()
        {
            byte actual = 1;
            int expected = 2;
            Assert.Throws<AssertException>(() => actual.AssertIsEqualTo(expected));
        }
        
        [Fact]
        public void byte_Ok()
        {
            byte actual = 2;
            int expected = 2;
            actual.AssertIsEqualTo(expected);
        }
    }
}
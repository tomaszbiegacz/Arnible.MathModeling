using System;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsNotEqualToExtensionsTests
    {
        [Fact]
        public void Value_Error()
        {
            try
            {
                2.AssertIsNotEqualTo(2);
                throw new Exception("I should not get here");
            }
            catch(AssertException)
            {
                // all is ok
            }
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
            try
            {
                SomeEnum.a.AssertIsNotEqualToEnum(SomeEnum.a);
                throw new Exception("I should not get here");
            }
            catch(AssertException)
            {
                // all is ok
            }
        }
        
        [Fact]
        public void Enum_Ok()
        {
            SomeEnum.a.AssertIsNotEqualToEnum(SomeEnum.b);
        }
    }
}
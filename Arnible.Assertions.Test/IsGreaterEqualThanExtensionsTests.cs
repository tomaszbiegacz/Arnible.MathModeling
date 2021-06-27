using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsGreaterEqualThanExtensionsTests
    {
        [Fact]
        public void Error()
        {
            Assert.Throws<AssertException>(() => 3.AssertIsGreaterEqualThan(4));
        }
        
        [Fact]
        public void Ok()
        {
            4.AssertIsGreaterEqualThan(4);
        }
        
        [Fact]
        public void ushort_int_Error()
        {
            ushort v = 3;
            Assert.Throws<AssertException>(() => v.AssertIsGreaterEqualThan(4));
        }
    }
}
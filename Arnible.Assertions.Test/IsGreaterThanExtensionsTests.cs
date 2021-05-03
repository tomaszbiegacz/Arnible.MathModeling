using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsGreaterThanExtensionsTests
    {
        [Fact]
        public void Error()
        {
            Assert.Throws<AssertException>(() => 3.AssertIsGreaterThan(3));
        }

        [Fact]
        public void Ok()
        {
            4.AssertIsGreaterThan(3);
        }
        
        [Fact]
        public void Ushort_Error()
        {
            ushort actual = 3;
            Assert.Throws<AssertException>(() => actual.AssertIsGreaterThan(3));
        }
        
        [Fact]
        public void Ushort_Ok()
        {
            ushort actual = 3;
            actual.AssertIsGreaterThan(2);
        }
    }
}
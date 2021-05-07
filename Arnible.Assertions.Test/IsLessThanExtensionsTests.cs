using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsLessThanExtensionsTests
    {
        [Fact]
        public void Error()
        {
            Assert.Throws<AssertException>(() => 3.AssertIsLessThan(3));
        }
        
        [Fact]
        public void Ok()
        {
            2.AssertIsLessThan(3);
        }
    }
}
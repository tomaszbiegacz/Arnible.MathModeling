using Xunit;

namespace Arnible.Assertions.Test
{
    public class AllExtensionsTests
    {
        [Fact]
        public void All_Error()
        {
            Assert.Throws<AssertException>(() => new int[] {1, 2}.AssertAll(i => i != 2));
        }
        
        [Fact]
        public void All()
        {
            new int[] {1, 2}.AssertAll(i => i > 0);
        }
    }
}
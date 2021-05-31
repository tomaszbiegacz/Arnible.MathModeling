using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsLessEqualThanExtensionsTests
    {
        [Fact]
        public void Error()
        {
            Assert.Throws<AssertException>(() => 3.AssertIsLessEqualThan(2));
        }
        
        [Fact]
        public void Ok()
        {
            2.AssertIsLessEqualThan(2);
        }
        
        [Fact]
        public void UShort_Error()
        {
            ushort value = 3;
            Assert.Throws<AssertException>(() => value.AssertIsLessEqualThan(2));
        }
        
        [Fact]
        public void UShort_Ok()
        {
            ushort value = 2;
            value.AssertIsLessEqualThan(2);
        }
    }
}
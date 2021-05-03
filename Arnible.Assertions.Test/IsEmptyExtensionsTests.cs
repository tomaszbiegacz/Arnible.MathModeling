using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class IsEmptyExtensionsTests
    {
        [Fact]
        public void Enumerable_Error()
        {
            IEnumerable<int> val = new int[] {1, 2};
            Assert.Throws<AssertException>(() => val.AssertIsEmpty());
        }
        
        [Fact]
        public void Enumerable_Ok()
        {
            IEnumerable<int> val = new int[] {};
            val.AssertIsEmpty();
        }
        
        [Fact]
        public void string_Error()
        {
            Assert.Throws<AssertException>(() => "test".AssertIsEmpty());
        }
        
        [Fact]
        public void string_Ok()
        {
            "".AssertIsEmpty();
        }
        
        [Fact]
        public void ReadOnlyCollection_Error()
        {
            IReadOnlyCollection<int> val = new int[] {1, 2};
            Assert.Throws<AssertException>(() => val.AssertIsEmpty());
        }
        
        [Fact]
        public void ReadOnlyCollection_Ok()
        {
            IReadOnlyCollection<int> val = new int[] {};
            val.AssertIsEmpty();
        }
    }
}
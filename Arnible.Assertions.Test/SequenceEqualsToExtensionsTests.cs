using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class SequenceEqualsToExtensionsTests
    {
        [Fact]
        public void IEnumerable_Error()
        {
            IEnumerable<int> actual = new int[] {1, 2};
            Assert.Throws<AssertException>(() => actual.AssertSequenceEqualsTo(new int[] {1, 3}));
        }
        
        [Fact]
        public void IEnumerable_Ok()
        {
            IEnumerable<int> actual = new int[] {1, 2};
            actual.AssertSequenceEqualsTo(new int[] {1, 2});
        }

        [Fact]
        public void Span_Error()
        {
            Span<int> actual = stackalloc int[] {1, 2};
            try
            {
                actual.AssertSequenceEqualsTo(stackalloc int[] {1, 3});
                throw new Exception("I should not got here");
            }
            catch (AssertException)
            {
                // all is fine
            }
        }
        
        [Fact]
        public void Span_Ok()
        {
            Span<int> actual = stackalloc int[] {1, 2};
            actual.AssertSequenceEqualsTo(stackalloc int[] {1, 2});
        }
        
        [Fact]
        public void ReadOnlySpan_Ok()
        {
            ReadOnlySpan<int> actual = stackalloc int[] {1, 2};
            actual.AssertSequenceEqualsTo(stackalloc int[] {1, 2});
        }
        
        [Fact]
        public void ReadOnlySpan_Error()
        {
            ReadOnlySpan<int> actual = stackalloc int[] {1, 2};
            try
            {
                actual.AssertSequenceEqualsTo(stackalloc int[] {1, 3});
                throw new Exception("I should not got here");
            }
            catch (AssertException)
            {
                // all is fine
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Assertions.Test
{
    public class AllExtensionsTests
    {
        [Fact]
        public void Span_All_int_Error()
        {
            Span<int> toCheck = new int[] {1, 2};
            try
            {
                toCheck.AssertAll((in int i) => i != 2);
                throw new Exception("I should not get here");
            }
            catch(AssertException)
            {
                // all is OK
            }
        }
        
        [Fact]
        public void Span_int_All()
        {
            Span<int> toCheck = new int[] {1, 2};
            toCheck.AssertAll((in int i) => i > 0);
        }
        
        [Fact]
        public void Span_bool_All()
        {
            Span<bool> toCheck = new bool[] {true, true};
            toCheck.AssertAll();
        }
        
        [Fact]
        public void ReadOnlySpan_bool_Error()
        {
            ReadOnlySpan<bool> toCheck = new bool[] {true, false};
            try
            {
                toCheck.AssertAll();
                throw new Exception("Something is wrong");
            }
            catch(AssertException)
            {
                // all is OK
            }
        }
    }
}
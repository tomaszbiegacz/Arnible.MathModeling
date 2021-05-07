using System;
using Xunit;

namespace Arnible.Test
{
  public class ReadOnlyArrayTests
  {
    [Fact]
    public void double_Basic()
    {
      ReadOnlyArray<double> src = default;
      Assert.Equal(0, src.Length);
      Assert.True(src.IsEmpty);
      Assert.True(src.Equals(default));

      ReadOnlyArray<double> other = new double[] { 1, 2 };
      Assert.Equal(2, other.Length);
      Assert.Equal(1, other[0]);
      Assert.Equal(2, other[1]);
      
      Assert.False(other.IsEmpty);
      Assert.False(other.Equals(src));
      Assert.NotEqual(other.GetHashCode(), src.GetHashCode());
      
      ReadOnlySpan<double> otherSpan = other;
      Assert.Equal(2, otherSpan.Length);
      Assert.Equal(1, otherSpan[0]);
      Assert.Equal(2, otherSpan[1]);
      
      Assert.Equal(2, other.AsList().Count);
      Assert.Equal(1, other.AsList()[0]);
      Assert.Equal(2, other.AsList()[1]);
      
      ReadOnlyArray<double> other2 = new double[] { 1, 1+1 };
      Assert.True(other.Equals(other2));
      Assert.Equal(other.GetHashCode(), other2.GetHashCode());
      
      ReadOnlyArray<double> other3 = new double[] { 1, 2, 3 };
      Assert.False(other.Equals(other3));
      Assert.False(other == other3);
      Assert.True(other != other3);
      Assert.NotEqual(other.GetHashCode(), other3.GetHashCode());
      Assert.Equal("[1,2,3]", other3.ToString());
      
      Assert.Equal(1, other3.First);
      Assert.Equal(3, other3.Last);
      
      ReadOnlyArray<double> other4 = new double[] { 1, 3 };
      Assert.False(other.Equals(other4));
      Assert.NotEqual(other.GetHashCode(), other4.GetHashCode());
      Assert.False(other.Equals((object?)null));

      ushort pos = 0;
      foreach (double val in other4)
      {
        switch (pos)
        {
          case 0:
            Assert.Equal(1, val);
            break;
          case 1:
            Assert.Equal(3, val);
            break;
          default:
            throw new Exception();
        }

        pos++;
      }
    }
  }
}
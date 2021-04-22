using Xunit;

namespace Arnible.Linq.Algebra.Test
{
  public class SumDefensiveTests
  {
    [Fact]
    public void ushort_SumDefensive()
    {
      var values = new ushort[] { 1, 2, 3 };
      Assert.Equal<uint>(6, values.SumDefensive());
    }
    
    [Fact]
    public void uint_SumDefensive()
    {
      var values = new uint[] { 1, 2, 3 };
      Assert.Equal<ulong>(6, values.SumDefensive());
    }
    
    [Fact]
    public void double_SumDefensive()
    {
      var values = new double[] { 1, 2, 3 };
      Assert.Equal(6.0, values.SumDefensive());
    }
  }
}
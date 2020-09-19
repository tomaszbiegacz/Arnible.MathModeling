using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Algebra.Test
{
  public class SignArrayTests
  {
    private static SignArray Create(params sbyte[] values)
    {
      return new SignArray(values);
    }
    
    // 1 -> 3
    
    [Fact]
    public void Orthogonal_1_True()
    {
      IsTrue(Create(0).GetIsOrthogonal());
      IsTrue(Create(1).GetIsOrthogonal());
    }
    
    [Fact]
    public void Orthogonal_1_False()
    {
      IsFalse(Create(-1).GetIsOrthogonal());
    }
    
    // 2 -> 9
    
    [Fact]
    public void Orthogonal_2_True()
    {
      IsTrue(Create(0, 0).GetIsOrthogonal());
      IsTrue(Create(0, 1).GetIsOrthogonal());
      IsTrue(Create(1, 0).GetIsOrthogonal());
      IsTrue(Create(1, 1).GetIsOrthogonal());
      IsTrue(Create(-1, 1).GetIsOrthogonal());
    }
    
    [Fact]
    public void Orthogonal_2_False()
    {
      IsFalse(Create(0, -1).GetIsOrthogonal());
      IsFalse(Create(-1, 0).GetIsOrthogonal());
      IsFalse(Create(1, -1).GetIsOrthogonal());
      IsFalse(Create(-1, -1).GetIsOrthogonal());
    }
    
    // 3 -> 27
    
    [Fact]
    public void Orthogonal_3_True()
    {
      IsTrue(Create(0, 0, 0).GetIsOrthogonal());
      
      IsTrue(Create(0, 0, 1).GetIsOrthogonal());
      IsTrue(Create(0, 1, 0).GetIsOrthogonal());
      IsTrue(Create(1, 0, 0).GetIsOrthogonal());
      
      IsTrue(Create(0, 1, 1).GetIsOrthogonal());
      IsTrue(Create(0, -1, 1).GetIsOrthogonal());
      
      IsTrue(Create(1, 0, 1).GetIsOrthogonal());
      IsTrue(Create(-1, 0, 1).GetIsOrthogonal());
      
      IsTrue(Create(1, 1, 0).GetIsOrthogonal());
      IsTrue(Create(-1, 1, 0).GetIsOrthogonal());
      
      IsTrue(Create(1, 1, 1).GetIsOrthogonal());
      IsTrue(Create(1, -1, 1).GetIsOrthogonal());
      IsTrue(Create(-1, 1, 1).GetIsOrthogonal());
      IsTrue(Create(-1, -1, 1).GetIsOrthogonal());
    }
    
    [Fact]
    public void Orthogonal_3_False()
    {
      IsFalse(Create(0, 0, -1).GetIsOrthogonal());
      IsFalse(Create(0, -1, 0).GetIsOrthogonal());
      IsFalse(Create(-1, 0, 0).GetIsOrthogonal());
      
      IsFalse(Create(0, -1, -1).GetIsOrthogonal());
      IsFalse(Create(0, 1, -1).GetIsOrthogonal());
      
      IsFalse(Create(-1, 0, -1).GetIsOrthogonal());
      IsFalse(Create(1, 0, -1).GetIsOrthogonal());
      
      IsFalse(Create(-1, -1, 0).GetIsOrthogonal());
      IsFalse(Create(1, -1, 0).GetIsOrthogonal());
      
      IsFalse(Create(-1, -1, -1).GetIsOrthogonal());
      IsFalse(Create(-1, 1, -1).GetIsOrthogonal());
      IsFalse(Create(1, -1, -1).GetIsOrthogonal());
      IsFalse(Create(1, 1, -1).GetIsOrthogonal());
    }
  }
}
using System;
using Arnible.MathModeling.Export;
using Arnible.MathModeling.xunit;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Optimization.Test
{
  public class UnimodalGoldenSecantTests
  {
    private readonly IMathModelingLogger _logger = new DeafLogger();

    [Fact]
    public void SquareTest()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(2);
      
      var method = new UnimodalGoldenSecant(f: f, a: a, b: b, _logger);
      AreExactlyEqual(2, method.X);
      AreExactlyEqual(4, method.Y);
      
      IsTrue(method.MoveNext());
      AreEqual(1, method.X);
      AreEqual(3, method.Y);
      
      IsFalse(method.MoveNext());
    }
    
    [Fact]
    public void SinTest()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(-1.3 * Math.PI);
      var b = f.ValueWithDerivative(0.4 * Math.PI);
      
      var method = new UnimodalGoldenSecant(f: f, a: a, b: b, _logger);
      AreExactlyEqual(a.X, method.X);
      AreExactlyEqual(a.Y, method.Y);
      
      Number width = method.Width;
      Number value = method.Y;

      uint i = 0;
      while(!method.IsOptimal)
      {
        i++;
        IsTrue(method.MoveNext());
        IsLowerThan(width, method.Width);
        IsLowerEqualThan(value, method.Y);

        width = method.Width;
        value = method.Y;
      }
      
      AreEqual(2, method.Y);
      AreEqual(6, i);
    }
    
    [Fact]
    public void SinTest_IncreasingAtCorners()
    {
      var f = new SinTestFunction();
      var a = f.ValueWithDerivative(-1.7 * Math.PI);
      var b = f.ValueWithDerivative(0.4 * Math.PI);
      
      IsPositive(a.First);
      IsPositive(b.First);
      
      var method = new UnimodalGoldenSecant(f: f, a: a, b: b, _logger);
      AreExactlyEqual(a.X, method.X);
      AreExactlyEqual(a.Y, method.Y);
      
      Number width = method.Width;
      Number value = method.Y;

      uint i = 0;
      while(!method.IsOptimal)
      {
        i++;
        IsTrue(method.MoveNext());
        IsLowerThan(width, method.Width);
        IsLowerEqualThan(value, method.Y);

        width = method.Width;
        value = method.Y;
      }
      
      AreEqual(2, method.Y);
      AreEqual(7, i);
    }
  }
}
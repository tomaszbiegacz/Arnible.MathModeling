using System;
using Arnible.MathModeling.Export;
using Arnible.MathModeling.xunit;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Optimization.Test
{
  public class UnimodalSecantTests
  {
    private readonly IMathModelingLogger _logger = new DeafLogger();

    [Fact]
    public void SquareTest()
    {
      var f = new SquareTestFunction();
      var a = f.ValueWithDerivative(-1);
      var b = f.ValueWithDerivative(2);
      
      var method = new UnimodalSecant(f: f, a: a, b: b, _logger);
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
      
      var method = new UnimodalSecant(f: f, a: a, b: b, _logger);
      AreExactlyEqual(a.X, method.X);
      AreExactlyEqual(a.Y, method.Y);

      Number currentY = method.Y;
      IsTrue(method.MoveNext());
      IsLowerThan(currentY, method.Y);

      for (uint i = 0; i < 4; ++i)
      {
        AreNotEqual(2, method.Y);
        IsTrue(method.MoveNext());
      }
      AreEqual(2, method.Y);
    }
  }
}
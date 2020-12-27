using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;
using static Arnible.MathModeling.xunit.AssertHelpers;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberTranslationVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberTranslationVector v = default;
      IsTrue(v == 0);
      AreEqual(1u, v.Length);
      AreExactlyEqual(0, v[0]);
      AreEqual(0, v.GetLengthSquare());
      AreEqual("0", v.ToString());

      AreEqual(default, v);
      AreEqual(default, new NumberTranslationVector());
      AreEqual(default, new NumberTranslationVector(new Number[0]));
    }

    [Fact]
    public void Constructor_Single()
    {
      NumberTranslationVector v = 2;
      IsFalse(v == 0);
      IsTrue(v == 2);
      IsFalse(v != 2);
      AreExactlyEqual(2d, v[0]);
      AreEqual(1u, v.Length);
      AreEqual(4, v.GetLengthSquare());
      AreEqual("2", v.ToString());
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberTranslationVector v = new NumberTranslationVector(2, 3, 4);
      IsFalse(v == 0);
      AreEquals(new Number[] { 2, 3, 4 }, v);
      AreEqual(3u, v.Length);
      AreEqual(4+9+16, v.GetLengthSquare());
      AreEqual("[2 3 4]", v.ToString());
    }
    
    [Fact]
    public void Constructor_FromArray()
    {
      ValueArray<Number> src = new Number[] {1, 3};
      ValueArray<Number> dst = new Number[] {3, 5};

      NumberTranslationVector v = new NumberTranslationVector(startingPoint: src, destinationPoint: dst);
      AreEqual(dst, v.Translate(src));
    }

    [Fact]
    public void Translate_Vector()
    {
      var t = new NumberTranslationVector(2, 3, 4);
      var v = new NumberVector(1, 2, 3);
      AreEqual(new NumberVector(3, 5, 7), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorLess()
    {
      var t = new NumberTranslationVector(2, 3);
      var v = new NumberVector(1, 2, 3);
      AreEqual(new NumberVector(3, 5, 3), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorMore()
    {
      var t = new NumberTranslationVector(2, 3, 4);
      var v = new NumberVector(1, 2);
      AreEqual(new NumberVector(3, 5, 4), t.Translate(v));
    }

    [Fact]
    public void Translate_Array()
    {
      var t = new NumberTranslationVector(2, 3, 4);
      var v = new Number[] { 1, 2, 3 };
      AreEqual(new Number[] { 3, 5, 7 }, t.Translate(v));
    }

    [Fact]
    public void Translate_ArrayLess()
    {
      var t = new NumberTranslationVector(2, 3);
      var v = new Number[] { 1, 2, 3 };
      AreEqual(new Number[] { 3, 5, 3 }, t.Translate(v));
    }
    
    [Fact]
    public void GetNormalized_Zero()
    {
      NumberTranslationVector v = default;
      AreEqual(v, v.GetNormalized());
    }
    
    [Fact]
    public void GetNormalized_One()
    {
      NumberTranslationVector v = new NumberTranslationVector(0, 1);
      AreEqual(v, v.GetNormalized());
    }
    
    [Fact]
    public void GetNormalized_Simple()
    {
      NumberTranslationVector v = new NumberTranslationVector(2);
      AreEqual(new NumberTranslationVector(1), v.GetNormalized());
      AreNotEqual(v, v.GetNormalized());
    }
    
    [Fact]
    public void GetNormalized_Complex()
    {
      NumberTranslationVector v = new NumberTranslationVector(3, 4);
      AreEqual(new NumberTranslationVector(0.6, 0.8), v.GetNormalized());
    }
  }
}

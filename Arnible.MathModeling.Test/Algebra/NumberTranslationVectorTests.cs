using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Arnible.MathModeling.Algebra.Test
{
  public class NumberTranslationVectorTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberTranslationVector v = default;
      Assert.True(v == 0);
      Assert.Equal(1u, v.Length);
      Assert.Equal(0, v[0]);
      Assert.Equal("0", v.ToString());

      Assert.Equal(default, v);
      Assert.Equal(default, new NumberTranslationVector());
      Assert.Equal(default, new NumberTranslationVector(new Number[0]));
    }

    [Fact]
    public void Constructor_Single()
    {
      NumberTranslationVector v = 2;
      Assert.False(v == 0);
      Assert.True(v == 2);
      Assert.False(v != 2);
      Assert.Equal(2, v[0]);
      Assert.Equal(1u, v.Length);
      Assert.Equal("2", v.ToString());
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberTranslationVector v = new NumberTranslationVector(2, 3, 4);
      Assert.False(v == 0);
      Assert.Equal(v, new Number[] { 2, 3, 4 });
      Assert.Equal(3u, v.Length);
      Assert.Equal("[2 3 4]", v.ToString());      
    }

    [Fact]
    public void Translate_Vector()
    {
      var t = new NumberTranslationVector(2, 3, 4);
      var v = new NumberVector(1, 2, 3);
      Assert.Equal(new NumberVector(3, 5, 7), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorLess()
    {
      var t = new NumberTranslationVector(2, 3);
      var v = new NumberVector(1, 2, 3);
      Assert.Equal(new NumberVector(3, 5, 3), t.Translate(v));
    }

    [Fact]
    public void Translate_VectorMore()
    {
      var t = new NumberTranslationVector(2, 3, 4);
      var v = new NumberVector(1, 2);
      Assert.Equal(new NumberVector(3, 5, 4), t.Translate(v));
    }

    [Fact]
    public void Translate_Array()
    {
      var t = new NumberTranslationVector(2, 3, 4);
      var v = new NumberArray(1, 2, 3);
      Assert.Equal(new NumberArray(3, 5, 7), t.Translate(v));
    }

    [Fact]
    public void Translate_ArrayLess()
    {
      var t = new NumberTranslationVector(2, 3);
      var v = new NumberArray(1, 2, 3);
      Assert.Equal(new NumberArray(3, 5, 3), t.Translate(v));
    }
  }
}

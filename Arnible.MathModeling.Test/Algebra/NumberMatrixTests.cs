using Arnible.MathModeling.Algebra;
using Xunit;

namespace Arnible.MathModeling.Test.Algebra
{
  public class NumberMatrixTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberMatrix v = default;
      Assert.True(v.IsZero);
      Assert.Equal(0u, v.Width);
      Assert.Equal(0u, v.Height);
      Assert.Equal("{}", v.ToString());

      Assert.Equal(default, v);
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberMatrix v = new NumberMatrix(new Number[,] {
        { 2, 3, 4 },
        { 5, 6, 7 }
      });
      Assert.False(v.IsZero);

      Assert.Equal(3u, v.Width);
      Assert.Equal(v.Column(0), new Number[] { 2, 5 });
      Assert.Equal(v.Column(1), new Number[] { 3, 6 });
      Assert.Equal(v.Column(2), new Number[] { 4, 7 });

      Assert.Equal(3, v[0, 1]);

      Assert.Equal(2u, v.Height);
      Assert.Equal(v.Row(0), new Number[] { 2, 3, 4 });
      Assert.Equal(v.Row(1), new Number[] { 5, 6, 7 });

      Assert.Equal("{[2 3 4] [5 6 7]}", v.ToString());

      NumberMatrix v1 = new NumberMatrix(new Number[,] { { 1, 2, 3 }, { 4, 5, 6 } });
      NumberMatrix v2 = new NumberMatrix(new Number[,] { { 3, 5, 7 }, { 9, 11, 13 } });
      Assert.Equal(v2, v + v1);
    }

    [Fact]
    public void NotEqual_Values()
    {
      Assert.NotEqual(new NumberMatrix(new Number[,] { { 1, 2 } }), new NumberMatrix(new Number[,] { { 1, 3 } }));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      Assert.NotEqual(default, new NumberMatrix(new Number[,] { { 0 } }));
    }

    [Fact]
    public void Equal_Rounding()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 1, 1, 0 } }), new NumberMatrix(new Number[,] { { 1, 1, 8.65956056235496E-17 } }));
    }

    [Fact]
    public void CreateUniform()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 2, 2, 2 }, { 2, 2, 2 } }), NumberMatrix.Repeat(element: 2, width: 3, height: 2));
    }

    [Fact]
    public void Minus()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 0, -1, -2 } }), new NumberMatrix(new Number[,] { { 1, 2, 3 } }) - new NumberMatrix(new Number[,] { { 1, 3, 5 } }));
    }

    [Fact]
    public void AppendColumn()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 0, 1, 5 }, { 1, 2, 5 } }), new NumberMatrix(new Number[,] { { 0, 1 }, { 1, 2 } }).AppendColumn(5));
    }

    [Fact]
    public void RemoveColumn()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 0, 1 }, { 1, 2 } }), new NumberMatrix(new Number[,] { { 0, 1, 5 }, { 1, 2, 5 } }).RemoveColumn(2));
    }

    [Fact]
    public void DuplicateRow()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 0, 1 }, { 1, 2 }, { 1, 2 } }), new NumberMatrix(new Number[,] { { 0, 1 }, { 1, 2 } }).DuplicateRow(1));
    }

    [Fact]
    public void RemoveRow()
    {
      Assert.Equal(new NumberMatrix(new Number[,] { { 0, 1 }, { 1, 2 } }), new NumberMatrix(new Number[,] { { 0, 1 }, { 5, 6 }, { 1, 2 } }).RemoveRow(1));
    }

  }
}

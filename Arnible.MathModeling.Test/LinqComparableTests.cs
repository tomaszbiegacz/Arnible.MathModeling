using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqComparableTests
  {
    [Fact]
    public void SequenceCompare_Equal()
    {
      Assert.Equal(0, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void SequenceCompare_SecondGreater()
    {
      Assert.Equal(1, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 0, 3 }));
    }

    [Fact]
    public void SequenceCompare_ThirdLower()
    {
      Assert.Equal(-1, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 2, 4 }));
    }

    readonly struct ForOrdering
    {
      public int Root { get; }
      public int Reminder { get; }

      public ForOrdering(int v)
      {
        Root = v / 2;
        Reminder = v % 2;
      }
    }

    [Fact]
    public void OrderByDescending_Default()
    {
      Assert.Equal(
        new[] { new ForOrdering(3), new ForOrdering(2), new ForOrdering(1) }, 
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root, i => i.Reminder));
    }

    [Fact]
    public void OrderByDescending_WithThen()
    {
      Assert.Equal(
        new[] { new ForOrdering(2), new ForOrdering(3), new ForOrdering(1) },
        (new[] { new ForOrdering(1), new ForOrdering(2), new ForOrdering(3) }).OrderByDescending(i => i.Root).ThenOrderBy(i => i.Reminder));
    }
  }
}

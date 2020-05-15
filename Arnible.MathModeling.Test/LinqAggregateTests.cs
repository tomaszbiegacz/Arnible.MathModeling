using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class LinqAggregateTests
  {
    [Fact]
    public void Zip()
    {
      Assert.Equal(new[] { 5, 7, 9 }, (new[] { 1, 2 }).Zip(new[] { 4, 5, 9 }, (a, b) => (a ?? 0) + (b ?? 0)));
    }

    [Fact]
    public void ZipDefensive()
    {
      Assert.Equal(new[] { 5, 7, 9 }, (new[] { 1, 2, 3 }).ZipDefensive(new[] { 4, 5, 6 }, (a, b) => a + b));
    }

    [Fact]
    public void ZipCommon_Dictionary()
    {
      var col1 = new Dictionary<char, uint>
      {
        { 'a', 1 }, {'b', 2}, {'c', 3}
      };
      var col2 = new Dictionary<char, uint>
      {
        { 'a', 3 }, {'d', 2}, {'c', 5}
      };

      var result = col1.ZipCommon(col2, Math.Min);

      Assert.Equal(2, result.Count);
      Assert.Equal(1u, result['a']);
      Assert.Equal(3u, result['c']);
    }

    [Fact]
    public void AggregateCombinations_1ChosenOf3()
    {
      Assert.Equal(new[] { 4d, 2d, 3d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(1, g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateCombinations_2ChosenOf3()
    {
      Assert.Equal(new[] { 8d, 12d, 6d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(2, g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateCombinations_3ChosenOf3()
    {
      Assert.Equal(new[] { 24d }, (new[] { 4d, 2d, 3d }).AggregateCombinations(3, g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateAllCombinations_3()
    {
      Assert.Equal(new[] { 4d, 2d, 3d, 8d, 12d, 6d, 24d }, (new[] { 4d, 2d, 3d }).AggregateCombinationsAll(g => g.ProductDefensive()));
    }

    [Fact]
    public void AggregateCommonBy()
    {
      var col1 = new[] { new VariableTerm('a', 1), new VariableTerm('b', 2), new VariableTerm('c', 3) };
      var col2 = new[] { new VariableTerm('a', 2), new VariableTerm('e', 2), new VariableTerm('c', 2) };
      var col3 = new[] { new VariableTerm('a', 3), new VariableTerm('f', 2), new VariableTerm('c', 4) };

      IEnumerable<IEnumerable<VariableTerm>> source = new[] { col1, col2, col3 };
      IDictionary<char, uint> result = source.AggregateCommonBy(kv => kv.Variable, kv => kv.Select(v => v.Power).MinDefensive());

      Assert.Equal(2, result.Count);
      Assert.Equal(1u, result['a']);
      Assert.Equal(2u, result['c']);
    }
  }
}

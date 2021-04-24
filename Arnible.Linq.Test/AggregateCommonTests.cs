using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class AggregateCommonTests
  {
    record VariableTerm(char Variable, uint Power)
    {
      public char Variable { get; } = Variable;
      public uint Power { get; } = Power;
    }
    
    [Fact]
    public void AggregateCommonBy()
    {
      var col1 = new[] { new VariableTerm('a', 1), new VariableTerm('b', 2), new VariableTerm('c', 3) };
      var col2 = new[] { new VariableTerm('a', 2), new VariableTerm('e', 2), new VariableTerm('c', 2) };
      var col3 = new[] { new VariableTerm('a', 3), new VariableTerm('f', 2), new VariableTerm('c', 4) };

      IEnumerable<IEnumerable<VariableTerm>> source = new[] { col1, col2, col3 };
      Dictionary<char, uint> result = source
        .AggregateCommonBy(
          kv => kv.Variable, 
          kv => kv.Select(v => v.Power).MinDefensive());

      Assert.Equal(2, result.Count);
      Assert.Equal(1u, result['a']);
      Assert.Equal(2u, result['c']);
    }
  }
}
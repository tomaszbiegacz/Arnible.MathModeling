using Arnible.MathModeling.Algebra;
using System;

namespace Arnible.MathModeling.Test.Export
{
  public struct TestGenericRecord<TOutput> where TOutput : struct, IEquatable<TOutput>
  {
    public TOutput Output { get; set; }

    public Number Error { get; set; }

    public NumberVector ErrorVector { get; set; }

    public TestGenericSubRecord<TOutput> SubOutput { get; set; }
  }
}

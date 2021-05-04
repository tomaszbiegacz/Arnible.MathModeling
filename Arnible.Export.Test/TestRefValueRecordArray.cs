using System;

namespace Arnible.Export.Test
{
  public ref struct TestRefValueRecordArray
  {
    public ReadOnlySpan<int> Value { get; set; }
  }
}
using System;

namespace Arnible.Export.Test
{
  public class ReadOnlyArrayWrapper<T> where T : IEquatable<T>
  {
    public ReadOnlyArray<T> Value { get; set; }
  }
}
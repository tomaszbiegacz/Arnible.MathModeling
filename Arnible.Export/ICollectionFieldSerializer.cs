using System;
using System.Collections.Generic;

namespace Arnible.Export
{
  public interface ICollectionFieldSerializer<TField>
  {
    void Write(in ReadOnlySpan<char> fieldName, in ReadOnlySpan<TField> field);
    void Write(in ReadOnlySpan<char> fieldName, TField[]? field);
    void Write(in ReadOnlySpan<char> fieldName, IReadOnlyCollection<TField>? field);
  }
}
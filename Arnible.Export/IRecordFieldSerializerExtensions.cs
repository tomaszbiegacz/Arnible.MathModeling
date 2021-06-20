using System;

namespace Arnible.Export
{
  public static class IRecordFieldSerializerExtensions
  {
    public static void Write(
      this IRecordFieldSerializer serializer,
      in ReadOnlySpan<char> fieldName,
      bool value)
    {
      serializer.Write(fieldName, value ? "yes" : "no");
    }
  }
}
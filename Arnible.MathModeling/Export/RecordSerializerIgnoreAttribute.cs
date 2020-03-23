using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class RecordSerializerIgnoreAttribute : Attribute
  {
    // intentionally empty
  }
}

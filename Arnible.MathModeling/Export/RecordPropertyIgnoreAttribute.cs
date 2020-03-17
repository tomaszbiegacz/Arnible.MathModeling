using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class RecordPropertyIgnoreAttribute : Attribute
  {
    // intentionally empty
  }
}

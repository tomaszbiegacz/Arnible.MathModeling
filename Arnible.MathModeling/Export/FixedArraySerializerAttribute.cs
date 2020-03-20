using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  public class FixedArraySerializerAttribute : Attribute
  {
    public FixedArraySerializerAttribute(uint size)
    {
      if(size <= 0)
      {
        throw new ArgumentException(nameof(size));
      }
      Size = size;
    }

    public uint Size { get; }
  }
}

using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class MediaTypeSpecificationAttribute : Attribute
  {
    public MediaTypeSpecificationAttribute(string fileExtension)
    {
      if (string.IsNullOrWhiteSpace(fileExtension) || !fileExtension.StartsWith("."))
      {
        throw new ArgumentException(nameof(fileExtension));
      }
      FileExtension = fileExtension.Trim();
    }

    public string FileExtension { get; }
  }
}

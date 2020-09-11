using System.Globalization;

namespace Arnible.MathModeling.Export
{
  public static class BuiltinSerializer
  {
    public static string AsString(in byte? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in sbyte? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in short? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }
    
    public static string AsString(in ushort? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in uint? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in int? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in ulong? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in long? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in float? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in double? src)
    {
      return src?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;
    }

    public static string AsString(in char? src)
    {
      if (src.HasValue)
      {
        return new string(new[] {src.Value});
      }
      else
      {
        return string.Empty;
      }
    }

    public static string AsString(in string? src)
    {
      return src ?? string.Empty;
    }
  }
}
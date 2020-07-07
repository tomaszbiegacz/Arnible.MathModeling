namespace Arnible.MathModeling
{
  public static class NumberMath
  {
    public static Number Sin(in Number a) => DoubleExtension.RoundedSin((double)a);

    public static Number Cos(in Number a) => DoubleExtension.RoundedCos((double)a);
  }
}

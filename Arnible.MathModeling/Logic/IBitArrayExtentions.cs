namespace Arnible.MathModeling.Logic
{
  public static class IBitArrayExtentions
  {
    public static uint GetEnabledCount(this IBitArray array)
    {
      return array.Where(v => v).Count();
    }
  }
}

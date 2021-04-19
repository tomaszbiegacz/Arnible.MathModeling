namespace Arnible.MathModeling
{
  public interface IValueObject
  {
    int GetHashCode();
    int GetHashCodeValue();

    string? ToString();
    string ToStringValue();
  }
}
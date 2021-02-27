namespace Arnible.MathModeling.Export
{
  public interface IMathModelingLogger
  {
    bool IsLoggerEnabled { get; }
    void Log(string message);
  }
}

namespace Arnible.Linq
{
  public delegate TResult FuncIn<T, out TResult>(in T arg);
}
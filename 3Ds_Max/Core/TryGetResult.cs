namespace Pandora._3Ds_Max.Core
{
  public static class TryGetResult
  {
    public static TryGetResult<T> NotFound<T>() => new TryGetResult<T>(false, default (T));

    public static TryGetResult<T> Found<T>(T result) => new TryGetResult<T>(true, result);
  }
}

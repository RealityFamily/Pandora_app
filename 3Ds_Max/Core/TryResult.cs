

using System;

namespace Pandora._3Ds_Max.Core
{
  public struct TryResult
  {
    public TryResult(Exception e) => this.Exception = e ?? throw new ArgumentNullException(nameof (e));

    public bool IsFaulted => this.Exception != null;

    public Exception Exception { get; }

    public override string ToString() => !this.IsFaulted ? "void" : this.Exception.ToString();
  }
}

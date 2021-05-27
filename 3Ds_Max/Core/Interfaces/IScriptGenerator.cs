using Pandora.MVVM.Models;

namespace Pandora._3Ds_Max.Core
{
  public interface IScriptGenerator
  {
    TryResult<string> Generate(Item item);
  }
}

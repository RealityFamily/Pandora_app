using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora._3Ds_Max.Core
{
    public struct TryGetResult<T>
    {
        public TryGetResult(bool isFound, T value)
        {
            this.IsFound = isFound;
            this.Value = value;
        }

        public bool IsFound { get; }

        public T Value { get; }
    }
    

}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pandora.Core
{
    class ObservableObject<T> : ObservableObject
    {
        public T Value
        {
            get { return _value; }
            set {
                Set<T>(() => this.Value, ref _value, value);
            }
        }

        private T _value = default;

        public void ObjectChanged()
        {
            base.PropertyChangedHandler.Invoke(null, null);
        }
    }
}

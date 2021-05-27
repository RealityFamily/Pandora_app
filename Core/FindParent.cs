using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Pandora.Core
{
    class FindParent
    {
        public static T Find<T> (FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement finder = element;
            while (finder.GetType() != typeof(T) && finder != null)
            {
                finder = (FrameworkElement)finder.Parent;
            }

            if (finder is T) {
                return finder as T;
            } else
            {
                return default(T);
            }
        }
    }
}

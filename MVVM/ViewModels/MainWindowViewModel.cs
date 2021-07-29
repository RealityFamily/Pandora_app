using Microsoft.Win32;
using Pandora._3Ds_Max;
using Pandora._3Ds_Max.Core;
using Pandora.Core;
using Pandora.DI;
using Pandora.MVVM.Views;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pandora.MVVM.ViewModels
{
    class MainWindowViewModel
    {
        ObservableObject<Page> _content = new ObservableObject<Page>();


        public MainWindowViewModel()
        {
            LocalServiceLocator locator = new LocalServiceLocator();
            if (locator.ApplicationConfig.HasMax())
            {
                if (!locator.ApplicationConfig.CheckAuth()) {
                    locator.UserViewModel.LogOut();
                    _content.Value = new AuthPage();
                } else
                {
                    _content.Value = new GroupPage(Color.FromArgb(255, 34, 44, 63), Color.FromArgb(255, 45, 55, 73), locator.MainPage.Groups);
                }
            } else
            {
                _content.Value = new No3DsMaxPage();
            }

            locator.UserViewModel.Token.PropertyChanged += (sender, e) =>
            {
                if (_content.Value is AuthPage && !string.IsNullOrEmpty(locator.UserViewModel.Token.Value))
                {
                    _content.Value = new GroupPage(Color.FromArgb(255, 34, 44, 63), Color.FromArgb(255, 45, 55, 73), locator.MainPage.Groups);
                }
            };
        }

        public ObservableObject<Page> Content
        {
            get { return _content; }
        }
    }
}

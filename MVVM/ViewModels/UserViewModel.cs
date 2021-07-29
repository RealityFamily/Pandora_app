using Pandora.Core;
using Pandora.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora.MVVM.ViewModels
{
    class UserViewModel
    {
        private ObservableObject<string> _token = new ObservableObject<string>();
        private ObservableObject<string> _user = new ObservableObject<string>();

        public UserViewModel()
        {
            _token.PropertyChanged += (sender, e) => {
                if (string.IsNullOrEmpty(_token.Value))
                {
                    new LocalServiceLocator().ItemInfoViewModel.ChoosedItem.Value = null;
                    new LocalServiceLocator().ApplicationConfig.CleanAuth(); 
                    new LocalServiceLocator().MainWindow.Content.Value = new Views.AuthPage();
                }
            };
        }

        public void LogOut()
        {
            _token.Value = null;
            _user.Value = "Войдите для полного доступа к моделям";
        }

        public ObservableObject<string> Token
        {
            get
            {
                return _token;
            }
        }
        public ObservableObject<string> User
        {
            get
            {
                return _user;
            }
        }
    }
}

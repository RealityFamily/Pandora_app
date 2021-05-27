using Pandora.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora.MVVM.ViewModels
{
    class UserViewModel
    {
        private ObservableObject<string> _token = new ObservableObject<string>();
        private ObservableObject<string> _user = new ObservableObject<string>();

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

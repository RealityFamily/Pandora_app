using Pandora._3Ds_Max.Core;
using Pandora.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora._3Ds_Max
{
    class ApplicationConfig
    {
        private MaxDetectionInRegistry _maxDetector;
        private RegisterMethods _registerMethods;

        public ApplicationConfig()
        {
            _registerMethods = new RegisterMethods();
            _maxDetector = new MaxDetectionInRegistry();

            if (!_registerMethods.CheckProgInRegister())
            {
                _registerMethods.AddProgToRegister(); 
            }
            new MaxStartupScriptCopier().Execute();
        }

        public bool HasMax()
        {
            TryResult<bool> result = _maxDetector.Detect();
            return result.Value;
        }

        public bool CheckAuth()
        {
            string token = _registerMethods.GetValueFromProg<string>("Token");
            string user = _registerMethods.GetValueFromProg<string>("UserName");
            if (!string.IsNullOrEmpty(token))
            {
                new LocalServiceLocator().UserViewModel.Token.Value = token;
                new LocalServiceLocator().UserViewModel.User.Value = user;
                return true;
            } else
            {
                return false;
            }
        }

        public bool AddAuth(string token, string username)
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(username)) {
                bool tokenAdd = !_registerMethods.AddValueToProg("Token", token);
                bool userAdd = !_registerMethods.AddValueToProg("UserName", username);

                if (tokenAdd) { new LocalServiceLocator().UserViewModel.Token.Value = token; }
                if (userAdd) { new LocalServiceLocator().UserViewModel.User.Value = username; }
                
                return tokenAdd && userAdd;
            }
            else
            {
                return false;
            }
        }

        public bool RefreshAuth(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                bool tokenAdd = _registerMethods.AddValueToProg("Token", token);
                return tokenAdd;
            }
            else
            {
                return false;
            }
        }

        public void CleanAuth()
        {
            _registerMethods.CleanValueFromProg("Token"); 
            _registerMethods.CleanValueFromProg("User");

        }
    }
}

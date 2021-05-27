using Microsoft.Win32;
using Pandora._3Ds_Max.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora._3Ds_Max
{
    class RegisterMethods
    {
        private static string ProgRegistrySubKey = "Pandora";
        private static string ProgRegestryRootKey = "SOFTWARE\\";
        private static string ProgRegistryKey = ProgRegestryRootKey + ProgRegistrySubKey;

        private IRegistryProvider provider;

        public RegisterMethods()
        {
            provider = new RegistryProvider();
        }

        public void AddProgToRegister()
        {
            provider.TrySetKey(ProgRegestryRootKey, ProgRegistrySubKey);
            provider.TrySetValue<string>(ProgRegistryKey, "InstallPath", AppDomain.CurrentDomain.BaseDirectory);
        }

        public bool CheckProgInRegister()
        {
            var check = provider.TryGetKey(ProgRegestryRootKey, ProgRegistrySubKey);
            return check.IsFound ? check.Value : check.IsFound;
        }

        public bool AddValueToProg<T>(string valueName, T value)
        {
            var check = provider.TrySetValue(ProgRegistryKey, valueName, value);
            return check.IsFaulted;
        }

        public T GetValueFromProg<T>(string valueName)
        {
            var check = provider.TryGetValue<T>(ProgRegistryKey, valueName);
            if (check.IsFound) {
                return check.Value;
            } else {
                return default(T);
            }
        }
    }
}

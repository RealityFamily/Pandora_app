using Microsoft.Win32;
using System;

namespace Pandora._3Ds_Max.Core
{
    public class RegistryProvider : IRegistryProvider
    {
        private readonly string HKCUName = "HKEY_CURRENT_USER\\";

        public TryGetResult<bool> TryGetKey(
            string rootKeyName,
            string keyName,
            RegistryHive hKey,
            RegistryView view)
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(hKey, view);

            if (!string.IsNullOrWhiteSpace(rootKeyName))
            {
                registryKey = registryKey.OpenSubKey(rootKeyName);
            }

            if (registryKey == null)
                return TryGetResult.NotFound<bool>();

            string[] subKeys = registryKey.GetSubKeyNames();
            bool result = Array.Exists(subKeys, key => key.Equals(keyName));

            registryKey.Close();
            return TryGetResult.Found(result);
        }

        public TryGetResult<T> TryGetValue<T>(
            string keyName,
            string valueName,
            RegistryHive hKey,
            RegistryView view)
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(hKey, view).OpenSubKey(keyName);

            if (registryKey == null)
                return TryGetResult.NotFound<T>();

            T result = (T)registryKey.GetValue(valueName, null);

            if (result == null) {
                foreach (string subKeyName in registryKey.GetSubKeyNames())
                {
                    result = (T)registryKey.OpenSubKey(subKeyName).GetValue(valueName, null);
                    if (result != null) break;
                }
            }
            
            registryKey.Close();
            return (object)result != null ? TryGetResult.Found<T>(result) : TryGetResult.NotFound<T>();
        }


        public TryResult TrySetKey(
            string rootKey,
            string keyName,
            RegistryHive hKey,
            RegistryView view)
        {
            try
            {
                RegistryKey registryKey = RegistryKey.OpenBaseKey(hKey, view);

                if (!string.IsNullOrWhiteSpace(rootKey))
                {
                    registryKey = registryKey.OpenSubKey(rootKey, true);
                }

                registryKey.CreateSubKey(keyName);
            }
            catch (Exception e)
            {
                return new TryResult(e);
            }
            return new TryResult();
        }

        public TryResult TrySetValue<T>(
            string key,
            string valueName,
            T value,
            RegistryHive hKey,
            RegistryView view)
        {
            try
            {
                RegistryKey registryKey = RegistryKey.OpenBaseKey(hKey, view);

                if (!string.IsNullOrWhiteSpace(key))
                {
                    registryKey = registryKey.OpenSubKey(key, true);
                }

                registryKey.SetValue(valueName, value);
            }
            catch (Exception ex)
            {
                return new TryResult(ex);
            }
            return new TryResult();
        }

        public TryResult TryDeleteValueHKCU(
          string subKey,
          string valueName,
          bool throwOnMissing)
        {
            try
            {
                if (subKey.StartsWith(this.HKCUName))
                    subKey = subKey.Remove(0, this.HKCUName.Length);
                using (RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, true))
                    registryKey?.DeleteValue(valueName, throwOnMissing);
            }
            catch (Exception ex)
            {
                return new TryResult(ex);
            }
            return new TryResult();
        }
    }
}

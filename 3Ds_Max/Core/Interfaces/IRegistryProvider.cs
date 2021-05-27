using Pandora._3Ds_Max.Core;
using Microsoft.Win32;

namespace Pandora._3Ds_Max.Core
{
    public interface IRegistryProvider
    {
        TryGetResult<bool> TryGetKey(
            string rootKeyName,
            string keyName,
            RegistryHive hKey = RegistryHive.CurrentUser,
            RegistryView view = RegistryView.Default);

        TryGetResult<T> TryGetValue<T>(
            string keyName,
            string valueName,
            RegistryHive hKey = RegistryHive.CurrentUser,
            RegistryView view = RegistryView.Default);


        TryResult TrySetKey(
            string rootKey,
            string keyName,
            RegistryHive hKey = RegistryHive.CurrentUser,
            RegistryView view = RegistryView.Default);

        TryResult TrySetValue<T>(
            string key,
            string valueName,
            T value,
            RegistryHive hKey = RegistryHive.CurrentUser,
            RegistryView view = RegistryView.Default);

        TryResult TryDeleteValueHKCU(string subKey, string valueName, bool throwOnMissing);
    }
}

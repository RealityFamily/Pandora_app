using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora._3Ds_Max.Core
{
    class MaxDetectionInRegistry
    {
        private readonly string MaxHKLMKeys = "SOFTWARE\\Autodesk\\3dsMax";
        private readonly string MaxLocationKeyName = "Location";
        private readonly IRegistryProvider registryProvider;

        public MaxDetectionInRegistry()
        {
            this.registryProvider = new RegistryProvider();
        }

        public TryResult<bool> Detect()
        {
            bool flag = false;
             TryGetResult<string> tryGetResult1 = this.registryProvider.TryGetValue<string>(this.MaxHKLMKeys, this.MaxLocationKeyName, RegistryHive.LocalMachine, RegistryView.Registry64);
            return new TryResult<bool>(tryGetResult1.IsFound && File.Exists(Path.Combine(tryGetResult1.Value, "3dsmax.exe")));
        }
    }
}

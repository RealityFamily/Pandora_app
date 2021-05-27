
using System;
using System.IO;

namespace Pandora._3Ds_Max.Core
{
    class MaxStartupScriptCopier
    {
        private readonly string AutodeskFolderName = "Autodesk\\3dsMax";
        private readonly string ScriptsStartupFolderName = "ENU\\scripts\\startup";
        private readonly string ScriptFileName = "pandora-loader.ms";
        private readonly string _autodeskAppDataFolder;
        private readonly string _scriptPath;

        public MaxStartupScriptCopier()
        {
            this._autodeskAppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), this.AutodeskFolderName);
            this._scriptPath = AppDomain.CurrentDomain.BaseDirectory + "/Scripts/3Ds_Max/pandora-loader.ms";
        }

        public void Execute()
        {
            if (!Directory.Exists(this._autodeskAppDataFolder))
                return;
            foreach (string directory in Directory.GetDirectories(this._autodeskAppDataFolder))
            {
                string str1 = Path.Combine(directory, this.ScriptsStartupFolderName);
                string str2 = Path.Combine(str1, this.ScriptFileName);
                if (!File.Exists(str2))
                    File.Copy(this._scriptPath, str2);
            }
        }
    }
}

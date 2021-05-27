using Pandora.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using static Pandora.MVVM.Models.Item;

namespace Pandora._3Ds_Max.Core
{
    public class ScriptGenerator : IScriptGenerator
    {
        private static readonly IReadOnlyDictionary<ItemType, string> ScriptNames = new Dictionary<ItemType, string>()
        {
            {
                ItemType.Model,
                "Model.ms"
            },
            {
                ItemType.Material,
                "MaxMaterial.ms"
            }
        };
        private readonly string templatesDirectory;
        private readonly string modelsDirectory;

        public ScriptGenerator()
        {
            this.templatesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "Model");
            this.modelsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models");
        }

        public TryResult<string> Generate(Item item)
        {
            try
            {
                string path1 = Path.Combine(this.templatesDirectory, ScriptNames[item.Type]);

                if (!File.Exists(path1))
                    return new TryResult<string>((Exception)new Exception("Template file not found: " + path1));

                string contents = File.ReadAllText(path1);
                if (item.Type == ItemType.Model)
                {
                    contents = contents.Replace("{ModelPath}", Path.Combine(modelsDirectory, item.Id, "Model") + "\\");
                }
                else if (item.Type == ItemType.Material)
                {
                    contents = contents.Replace("{ModelPath}", Path.Combine(modelsDirectory, item.Id, "MaxMaterial") + "\\");
                }

                string path2 = Path.Combine(modelsDirectory, item.Id, "Pandora_" + Guid.NewGuid().ToString() + ".ms");
                File.WriteAllText(path2, contents);
                return new TryResult<string>(path2);
            }
            catch (Exception ex)
            {
                return new TryResult<string>(new Exception("Unhandled exception", ex));
            }
        }
    }
}

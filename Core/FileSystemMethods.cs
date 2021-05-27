using Pandora.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Pandora.Core
{
    class FileSystemMethods
    {
        private string ItemsFolder;

        public FileSystemMethods()
        {
            ItemsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models");

            if (!Directory.Exists(ItemsFolder))
            {
                Directory.CreateDirectory(ItemsFolder);
            }
        }

        public void UnzipModel(Item item, string zipPath)
        {
            string itemPath = Path.Combine(ItemsFolder, item.Id);
            if (!Directory.Exists(itemPath))
            {
                ZipFile.ExtractToDirectory(zipPath, Path.Combine(ItemsFolder, item.Id));
                File.Delete(zipPath);

                using (StreamWriter sw = File.CreateText(Path.Combine(itemPath, "Info.json")))
                {
                    sw.WriteLine(JsonSerializer.Serialize(item));
                }
            }
        }

        public void DeleteModel(Item item)
        {
            string itemPath = Path.Combine(ItemsFolder, item.Id);
            if (Directory.Exists(itemPath))
            {
                Directory.Delete(itemPath, true);
            }
        }

        public List<string> GetLocalId()
        {
            List<string> itemIdList = new List<string>();
            string[] itemsId = Directory.GetDirectories(ItemsFolder);

            foreach (string id in itemsId)
            {
                itemIdList.Add(Path.GetFileName(id));
            }

            return itemIdList;
        }

        public Item GetLocalItemInfo(string itemId)
        {
            string[] itemsId = Directory.GetDirectories(ItemsFolder);

            foreach (string itemFiles in itemsId)
            {
                if (File.Exists(Path.Combine(itemFiles, "Info.json")) && itemId.Equals(Path.GetFileName(itemFiles)))
                {
                    return JsonSerializer.Deserialize<Item>(File.ReadAllText(Path.Combine(itemFiles, "Info.json")));
                }
            }

            return null;
        }
    }
}

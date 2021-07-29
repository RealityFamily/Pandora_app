using Microsoft.Win32;
using Pandora.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Pandora.Core
{
    class FileSystemMethods
    {
        public enum Filter
        {
            Model,
            Material,
            Photo
        }

        private string _itemsFolder;
        private string _zipFolder;

        public FileSystemMethods()
        {
            _itemsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models");
            _zipFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Zip");

            if (!Directory.Exists(_itemsFolder))
            {
                Directory.CreateDirectory(_itemsFolder);
            }
            if (!Directory.Exists(_zipFolder))
            {
                Directory.CreateDirectory(_zipFolder);
            }
        }

        public void UnzipModel(Item item, string zipPath)
        {
            string itemPath = Path.Combine(_itemsFolder, item.Id);
            if (!Directory.Exists(itemPath))
            {
                ZipFile.ExtractToDirectory(zipPath, Path.Combine(_itemsFolder, item.Id));
                File.Delete(zipPath);

                using (StreamWriter sw = File.CreateText(Path.Combine(itemPath, "Info.json")))
                {
                    sw.WriteLine(JsonSerializer.Serialize(item));
                }
            }
        }

        public void DeleteModel(Item item)
        {
            string itemPath = Path.Combine(_itemsFolder, item.Id);
            if (Directory.Exists(itemPath))
            {
                Directory.Delete(itemPath, true);
            }
        }

        public List<string> GetLocalId()
        {
            List<string> itemIdList = new List<string>();
            string[] itemsId = Directory.GetDirectories(_itemsFolder);

            foreach (string id in itemsId)
            {
                itemIdList.Add(Path.GetFileName(id));
            }

            return itemIdList;
        }

        public Item GetLocalItemInfo(string itemId)
        {
            string[] itemsId = Directory.GetDirectories(_itemsFolder);

            foreach (string itemFiles in itemsId)
            {
                if (File.Exists(Path.Combine(itemFiles, "Info.json")) && itemId.Equals(Path.GetFileName(itemFiles)))
                {
                    return JsonSerializer.Deserialize<Item>(File.ReadAllText(Path.Combine(itemFiles, "Info.json")));
                }
            }

            return null;
        }

        public void ZipLocalModel(string modelPath, List<string> materialPaths, string photoPath = null)
        {
            string finalPath = Path.Combine(_zipFolder, Path.GetFileNameWithoutExtension(modelPath) + ".zip");

            DirectoryInfo modelDir = Directory.CreateDirectory(Path.Combine(_zipFolder, "Temp", "Model"));
            File.Copy(modelPath, Path.Combine(modelDir.FullName, Path.GetFileNameWithoutExtension(modelPath) + ".pan"));

            if (materialPaths != null) {
                foreach (string path in materialPaths)
                {
                    File.Copy(path, Path.Combine(modelDir.FullName, Path.GetFileName(path)));
                }
            }

            if (!string.IsNullOrEmpty(photoPath))
            {
                File.Copy(photoPath, Path.Combine(modelDir.Parent.FullName, Path.GetFileName(photoPath)));
            }

            ZipFile.CreateFromDirectory(modelDir.Parent.FullName, finalPath);
            Directory.Delete(Path.Combine(_zipFolder, "Temp"), true);

            Process.Start(new ProcessStartInfo()
            {
                FileName = _zipFolder,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public void ZipModelsInFolder(string directory)
        {

        }

        public List<string> ChooseFile(Filter filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            switch (filter)
            {
                case Filter.Model:
                    openFileDialog.Title = "Выберите модель для выгрузки";
                    openFileDialog.Filter = "Max files (*.max)|*.max";
                    openFileDialog.Multiselect = false;
                    break;
                case Filter.Material:
                    openFileDialog.Title = "Выберите материалы для выгрузки";
                    openFileDialog.Filter = "Material files (*.jpg, *.png, *.psd)|*.jpg;*.png;*.psd";
                    openFileDialog.Multiselect = true;
                    break;
                case Filter.Photo:
                    openFileDialog.Title = "Выберите aото модели для выгрузки";
                    openFileDialog.Filter = "Image files (*.jpg, *.png)|*.jpg;*.png";
                    openFileDialog.Multiselect = false;
                    break;
            }

            bool? result = openFileDialog.ShowDialog();

            if (result.Value)
            {
                return openFileDialog.FileNames.ToList();
            } else
            {
                return new List<string>();
            }
        }
    }
}

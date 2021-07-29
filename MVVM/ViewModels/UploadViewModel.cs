using Pandora.Core;
using Pandora.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora.MVVM.ViewModels
{
    class UploadViewModel
    {
        ObservableObject<string> _modelFile = new ObservableObject<string>();
        ObservableObject<string> _photoFile = new ObservableObject<string>();
        ObservableObject<List<string>> _materialsFiles = new ObservableObject<List<string>>();
        ObservableObject<string> _directory = new ObservableObject<string>();

        public void CreateZIP()
        {
            if (!string.IsNullOrEmpty(_directory.Value))
            {

            } else if (!string.IsNullOrEmpty(_modelFile.Value))
            {
                new LocalServiceLocator().FileSystemMethods.ZipLocalModel(_modelFile.Value, _materialsFiles.Value, _photoFile.Value);
            }
        }

        public void ChooseModel()
        {
            var result = new LocalServiceLocator().FileSystemMethods.ChooseFile(FileSystemMethods.Filter.Model);
            if (result.Count > 0)
            {
                _modelFile.Value = result[0];
            } else
            {
                _modelFile.Value = null;
            }
        }

        public void ChooseMaterials()
        {
            var result = new LocalServiceLocator().FileSystemMethods.ChooseFile(FileSystemMethods.Filter.Material);
            if (result.Count > 0)
            {
                _materialsFiles.Value = result;
            }
            else
            {
                _materialsFiles.Value = null;
            }
        }

        public void ChoosePhoto()
        {
            var result = new LocalServiceLocator().FileSystemMethods.ChooseFile(FileSystemMethods.Filter.Photo);
            if (result.Count > 0)
            {
                _photoFile.Value = result[0];
            }
            else
            {
                _photoFile.Value = null;
            }
        }

        public ObservableObject<string> ModelFile
        {
            get
            {
                return _modelFile;
            }
        }
        public ObservableObject<string> PhotoFile
        {
            get
            {
                return _photoFile;
            }
        }
        public ObservableObject<List<string>> MaterialsFiles
        {
            get
            {
                return _materialsFiles;
            }
        }
        public ObservableObject<string> Directory
        {
            get
            {
                return _directory;
            }
        }
    }
}

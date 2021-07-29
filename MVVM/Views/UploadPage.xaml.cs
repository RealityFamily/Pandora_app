using Pandora.DI;
using Pandora.MVVM.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Pandora.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для UploadPage.xaml
    /// </summary>
    public partial class UploadPage : Page
    {
        UploadViewModel mViewModel;

        public UploadPage()
        {
            InitializeComponent();

            mViewModel = new LocalServiceLocator().UploadViewModel;

            mViewModel.ModelFile.PropertyChanged += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(mViewModel.ModelFile.Value))
                {
                    ModelFile.Text = mViewModel.ModelFile.Value;
                } else
                {
                    ModelFile.Text = "Выберете модель для выгрузки";
                }
            };

            mViewModel.PhotoFile.PropertyChanged += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(mViewModel.PhotoFile.Value))
                {
                    ImageFile.Text = mViewModel.PhotoFile.Value;
                }
                else
                {
                    ImageFile.Text = "Выберете фотографию модели для выгрузки";
                }
            };

            mViewModel.MaterialsFiles.PropertyChanged += (sender, e) =>
            {
                if (mViewModel.MaterialsFiles.Value == null || mViewModel.MaterialsFiles.Value.Count == 0)
                {
                    MaterialsFiles.Text = "Выберите материалы модели";
                }
                else
                {
                    MaterialsFiles.Text = "Выбрано " + mViewModel.MaterialsFiles.Value.Count + " файлов";
                }
            };

            mViewModel.ModelFile.Value = string.Empty;
            mViewModel.PhotoFile.Value = string.Empty;
            mViewModel.MaterialsFiles.Value = new List<string>();

        }

        private void Begin_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.CreateZIP();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.ChoosePhoto();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mViewModel.ChooseModel();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            mViewModel.ChooseMaterials();
        }
    }
}

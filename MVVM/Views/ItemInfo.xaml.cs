using Pandora._3Ds_Max.Core;
using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pandora.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для ItemInfo.xaml
    /// </summary>
    public partial class ItemInfo : UserControl
    {
        ItemInfoViewModel mViewModel;

        public ItemInfo()
        {
            InitializeComponent();

            mViewModel = new LocalServiceLocator().ItemInfoViewModel;

            mViewModel.ChoosedItem.PropertyChanged += (sender, e) =>
            {
                ItemName.Text = mViewModel.ChoosedItem.Value?.Name;
                AuthorText.Text = mViewModel.ChoosedItem.Value?.Author;
                WeightText.Text = (mViewModel.ChoosedItem.Value?.Size / 1024 / 1024).ToString() + " Мб";
                DescriptionText.Text = mViewModel.ChoosedItem.Value?.Description;
                largePhoto.Source = mViewModel.ChoosedItem.Value?.LargeImage;

                if (mViewModel.ChoosedItem.Value != null)
                {
                    if (mViewModel.ChoosedItem.Value.Contains)
                    {
                        DeleteButton();
                    }
                    else
                    {
                        DownloadButton();
                    }
                }
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.ChoosedItem.Value = null;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            largePhoto.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/resources/drawable/instruction.png"));
        }

        private void largePhoto_MouseEnter(object sender, MouseEventArgs e)
        {
            largePhoto.Source = mViewModel.ChoosedItem.Value.LargeImage;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.ActionItem();
        }

        private void DownloadButton()
        {
            Action.Resources["StableBackground"] = new SolidColorBrush(Color.FromArgb(255, 51, 130, 201));
            Action.Content = "Скачать";
        }
        private void DeleteButton()
        {
            Action.Resources["StableBackground"] = new SolidColorBrush(Colors.Red);
            Action.Content = "Удалить";
        }

        private void largePhoto_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mViewModel.ChoosedItem.Value != null && mViewModel.ChoosedItem.Value.Contains)
            {
                IScriptGenerator scriptGenerator = new ScriptGenerator();
                
                TryResult<string> _scriptPath = scriptGenerator.Generate(mViewModel.ChoosedItem.Value);

                Image img = (Image)e.OriginalSource;

                string sourceFile = _scriptPath.Value;
                DataObject dataObject = new DataObject();
                dataObject.SetFileDropList(new StringCollection()
                    {
                        sourceFile
                    });

                int num = (int)DragDrop.DoDragDrop((DependencyObject)img, (object)dataObject, DragDropEffects.Copy);
                File.Delete(sourceFile);
            }
        }
    }
}

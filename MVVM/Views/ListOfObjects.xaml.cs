using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.MVVM.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ListOfObjects.xaml
    /// </summary>
    public partial class ListOfObjects : UserControl
    {
        ItemsListViewModel mViewModel;

        public ListOfObjects(Color BackgroundColor)
        {
            InitializeComponent();

            Page.Background = new SolidColorBrush(BackgroundColor);

            LocalServiceLocator serviceLocator = new LocalServiceLocator();
            mViewModel = serviceLocator.ListOfGroupViewModel;

            mViewModel.ItemsCollection.PropertyChanged += (sender, e) =>
            {
                ListContainer.Children.Clear();
                if (mViewModel.ItemsCollection.Value != null)
                {
                    foreach (string key in mViewModel.ItemsCollection.Value.Keys)
                    {
                        ListContainer.Children.Add(new GroupOfItems(key, mViewModel.ItemsCollection.Value[key]));
                    }
                }
            };

            serviceLocator.ItemInfoViewModel.ChoosedItem.PropertyChanged += (sender, e) =>
            {
                if (serviceLocator.ItemInfoViewModel.ChoosedItem.Value == null)
                {
                    UnSelectAll();
                }
            };
        }

        public void UnSelectAll()
        {
            foreach (GroupOfItems child in ListContainer.Children)
            {
                StackPanel panel = (StackPanel)child.Content;
                ListBox box = (ListBox)panel.Children[1];
                box.SelectedIndex = -1;
            }
        }
    }
}

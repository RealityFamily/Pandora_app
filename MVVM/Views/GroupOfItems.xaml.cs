using Pandora.Core;
using Pandora.DI;
using Pandora.MVVM.Models;
using Pandora.Network;
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
    /// Логика взаимодействия для GroupOfItems.xaml
    /// </summary>
    public partial class GroupOfItems : UserControl
    {
        private string _title;
        private List<Item> _items;

        public GroupOfItems(string Title, List<Item> Items)
        {
            InitializeComponent();

            _title = Title;
            _items = Items;

            if (!string.IsNullOrWhiteSpace(_title))
            {
                GroupName.Text = _title;
            }
            else
            {
                GroupName.Visibility = Visibility.Collapsed;
            }
            GroupItems.ItemsSource = _items;
        }

        private void GroupItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null &&
                new LocalServiceLocator().ItemInfoViewModel.ChoosedItem.Value?.Id != ((Item)((ListBox)sender).SelectedItem).Id)
            {
                Item selected = (Item)((ListBox)sender).SelectedItem;
                FindParent.Find<ListOfObjects>((FrameworkElement)Parent).UnSelectAll();

                new LocalServiceLocator().ItemInfoViewModel.ChoosedItem.Value = Item.GetItemInfo(selected);
                ((ListBox)sender).SelectedItem = selected;
            }
        }
    }
}

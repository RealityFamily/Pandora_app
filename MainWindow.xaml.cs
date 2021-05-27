using MahApps.Metro.Controls;
using Pandora.DI;
using Pandora.MVVM.ViewModels;
using Pandora.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pandora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        MainWindowViewModel mViewModel;

        public string LogoPath = AppDomain.CurrentDomain.BaseDirectory + "/resources/drawable/logo.png";

        public MainWindow()
        {
            InitializeComponent();

            LocalServiceLocator serviceLocator = new LocalServiceLocator();
            mViewModel = serviceLocator.MainWindow;

            content.Content = mViewModel.Content.Value;
            mViewModel.Content.PropertyChanged += (sender, e) => {
                content.Content = mViewModel.Content.Value;
            };

            PersonName.Content = serviceLocator.UserViewModel.User.Value;
            serviceLocator.UserViewModel.User.PropertyChanged += (sender, e) => { PersonName.Content = serviceLocator.UserViewModel.User.Value; };

            serviceLocator.ItemInfoViewModel.ChoosedItem.PropertyChanged += (sender, e) =>
            {
                InfoColumn.Width = new GridLength(serviceLocator.ItemInfoViewModel.ChoosedItem.Value != null ? 532 : 0);
            };
        }

        private void PersonName_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(new LocalServiceLocator().UserViewModel.Token.Value)) {
                mViewModel.Content.Value = new AuthPage();
            }
        }
    }
}

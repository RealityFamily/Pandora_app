using MahApps.Metro.Controls;
using Pandora.DI;
using Pandora.MVVM.ViewModels;
using Pandora.MVVM.Views;
using System;
using System.Windows;

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
            if (new LocalServiceLocator().ApplicationConfig.HasMax()) {
                mViewModel.LogOut();
            }
        }
    }
}

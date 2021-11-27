using MahApps.Metro.Controls;
using Pandora.DI;
using Pandora.MVVM.ViewModels;
using Pandora.MVVM.Views;
using System;
using System.Windows;
using System.Windows.Media;

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

            String userField = serviceLocator.UserViewModel.User.Value;
            if (userField != null)
            {
                PersonName.Content = serviceLocator.UserViewModel.User.Value.Equals("Войдите для полного доступа к моделям") ? serviceLocator.UserViewModel.User.Value : "Выйти из: " + serviceLocator.UserViewModel.User.Value;
            }

            serviceLocator.UserViewModel.User.PropertyChanged += (sender, e) => 
            {
                PersonName.Content = serviceLocator.UserViewModel.User.Value.Equals("Войдите для полного доступа к моделям")? serviceLocator.UserViewModel.User.Value : "Выйти из: " + serviceLocator.UserViewModel.User.Value;
                AddButton.Visibility = serviceLocator.UserViewModel.User.Value.Equals("realityfamily")  ? Visibility.Visible : Visibility.Collapsed;
            };

            serviceLocator.ItemInfoViewModel.ChoosedItem.PropertyChanged += (sender, e) =>
            {
                InfoColumn.Width = new GridLength(serviceLocator.ItemInfoViewModel.ChoosedItem.Value != null ? 532 : 0);
            };
        }

        private void PersonName_Click(object sender, RoutedEventArgs e)
        {
            if (new LocalServiceLocator().ApplicationConfig.HasMax()) {
                new LocalServiceLocator().UserViewModel.LogOut();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new LocalServiceLocator().ItemInfoViewModel.ChoosedItem.Value = null;
            if (mViewModel.Content.Value is UploadPage) {
                mViewModel.Content.Value = new GroupPage(Color.FromArgb(255, 34, 44, 63), Color.FromArgb(255, 45, 55, 73), new LocalServiceLocator().MainPage.Groups);
            } else
            {
                mViewModel.Content.Value = new UploadPage();
            }
        }
    }
}

using Pandora.DI;
using Pandora.MVVM.ViewModels;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new LocalServiceLocator().MainWindow.Content.Value = new RegisterPage();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Name.Equals("UsernameTextBox"))
                {
                    UsernamePlaceholder.Visibility = Visibility.Collapsed;
                }
                if (((TextBox)sender).Name.Equals("PasswordTextBox"))
                {
                    PasswordPlaceholder.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Name.Equals("UsernameTextBox"))
                {
                    var textbox = sender as TextBox;
                    if (string.IsNullOrEmpty(textbox.Text))
                    {
                        UsernamePlaceholder.Visibility = Visibility.Visible;
                    }
                }
                if (((TextBox)sender).Name.Equals("PasswordTextBox"))
                {
                    var textbox = sender as TextBox;
                    if (string.IsNullOrEmpty(textbox.Text))
                    {
                        PasswordPlaceholder.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Text)) {
                InvalidCredentionals.Visibility = !AuthNetworkLogic.Auth(UsernameTextBox.Text, PasswordTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}

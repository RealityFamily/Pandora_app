using Pandora.DI;
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
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {

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
                if (((TextBox)sender).Name.Equals("EmailTextBox"))
                {
                    EmailPlaceholder.Visibility = Visibility.Collapsed;
                }
                if (((TextBox)sender).Name.Equals("RetryPasswordTextBox"))
                {
                    RetryPasswordPlaceholder.Visibility = Visibility.Collapsed;
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
                if (((TextBox)sender).Name.Equals("EmailTextBox"))
                {
                    var textbox = sender as TextBox;
                    if (string.IsNullOrEmpty(textbox.Text))
                    {
                        EmailPlaceholder.Visibility = Visibility.Visible;
                    }
                }
                if (((TextBox)sender).Name.Equals("RetryPasswordTextBox"))
                {
                    var textbox = sender as TextBox;
                    if (string.IsNullOrEmpty(textbox.Text))
                    {
                        RetryPasswordPlaceholder.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new LocalServiceLocator().MainWindow.Content.Value = new AuthPage();
        }
    }
}

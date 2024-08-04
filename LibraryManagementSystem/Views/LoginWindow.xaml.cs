using System;
using System.Windows;
using System.Windows.Controls;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(UserService userService, Action onSuccessfulLogin, Action onGoBack)
        {
            InitializeComponent();
            var viewModel = new LoginViewModel(userService, onSuccessfulLogin, onGoBack);
            viewModel.RequestClose += OnRequestClose;
            DataContext = viewModel;
        }

        private void OnRequestClose(object sender, EventArgs e)
        {
            Close();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel loginViewModel)
            {
                loginViewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
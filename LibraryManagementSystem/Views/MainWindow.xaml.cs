using System;
using System.Windows;
using System.Windows.Controls;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        private readonly ItemService _itemService;

        public MainWindow(UserService userService, ItemService itemService)
        {
            InitializeComponent();
            _userService = userService;
            _itemService = itemService;

            var viewModel = new MainWindowViewModel(_itemService, _userService);
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
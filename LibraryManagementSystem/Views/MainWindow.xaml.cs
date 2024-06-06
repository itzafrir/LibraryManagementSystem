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
            DataContext = new MainWindowViewModel(_itemService, _userService);
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
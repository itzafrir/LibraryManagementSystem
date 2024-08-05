using LibraryManagementSystem.ViewModels;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class AddUpdateUserWindow : Window
    {
        public AddUpdateUserWindow(User user, UserService userService)
        {
            InitializeComponent();
            var viewModel = new AddUpdateUserViewModel(user, userService, Close);
            DataContext = viewModel;
        }
    }
}
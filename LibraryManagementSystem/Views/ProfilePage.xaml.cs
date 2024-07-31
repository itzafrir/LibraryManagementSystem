using System;
using System.Windows;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class ProfilePage : Window
    {
        public ProfilePage(UserService userService, Action onGoBack, Action onLogout)
        {
            InitializeComponent();
            var viewModel = new ProfileViewModel(userService, onGoBack, onLogout);
            viewModel.RequestClose += OnRequestClose;
            DataContext = viewModel;
        }

        private void OnRequestClose(object sender, EventArgs e)
        {
            Close();
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;

        public IRelayCommand NavigateToCatalogCommand { get; }
        public IRelayCommand NavigateToProfileCommand { get; }
        public IRelayCommand NavigateToLoginCommand { get; }

        public event EventHandler RequestClose;

        public MainWindowViewModel(ItemService itemService, UserService userService)
        {
            _itemService = itemService;
            _userService = userService;

            NavigateToCatalogCommand = new RelayCommand(NavigateToCatalog);
            NavigateToProfileCommand = new RelayCommand(NavigateToProfile);
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
        }

        private void NavigateToCatalog()
        {
            var catalogPage = new Views.CatalogPage(_itemService, _userService)
            {
                DataContext = new CatalogViewModel(_itemService, _userService)
            };
            catalogPage.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToProfile()
        {
            if (_userService.IsUserLoggedIn())
            {
                var profilePage = new Views.ProfilePage
                {
                    DataContext = new ProfileViewModel(_userService)
                };
                profilePage.Show();
            }
            else
            {
                MessageBox.Show("Please log in to view your profile.", "Not Logged In", MessageBoxButton.OK, MessageBoxImage.Warning);
                NavigateToLogin();
            }
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToLogin()
        {
            var loginWindow = new Views.LoginWindow()
            {
                DataContext = new LoginViewModel(_userService)
            };
            loginWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System;
using LibraryManagementSystem.Views;

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
            NavigateToLoginCommand = new RelayCommand(() => NavigateToLogin(NavigateToMain));
        }

        private void NavigateToCatalog()
        {
            var catalogPage = new CatalogPage(_itemService, _userService)
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
                var profilePage = new ProfilePage(_userService, NavigateToMain)
                {
                    DataContext = new ProfileViewModel(_userService, NavigateToMain)
                };
                profilePage.Show();
            }
            else
            {
                NavigateToLogin(NavigateToProfile);
            }
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToLogin(Action onSuccessfulLogin, Action onGoBack = null)
        {
            var loginWindow = new Views.LoginWindow(_userService, onSuccessfulLogin, onGoBack ?? NavigateToMain)
            {
                DataContext = new LoginViewModel(_userService, onSuccessfulLogin, onGoBack ?? NavigateToMain)
            };
            ((LoginViewModel)loginWindow.DataContext).RequestClose += (_, __) => loginWindow.Close();
            loginWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToMain()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
        }
    }
}

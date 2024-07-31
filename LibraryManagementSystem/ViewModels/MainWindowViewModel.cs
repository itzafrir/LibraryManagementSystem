using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System;
using LibraryManagementSystem.Utilities.Enums;
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
        public IRelayCommand NavigateToAdminCommand { get; }

        public event EventHandler RequestClose;

        public MainWindowViewModel(ItemService itemService, UserService userService)
        {
            _itemService = itemService;
            _userService = userService;

            NavigateToCatalogCommand = new RelayCommand(NavigateToCatalog);
            NavigateToProfileCommand = new RelayCommand(NavigateToProfile);
            NavigateToLoginCommand = new RelayCommand(() => NavigateToLogin(NavigateToMain));
            NavigateToAdminCommand = new RelayCommand(NavigateToAdmin, CanNavigateToAdmin);
        }

        private void NavigateToCatalog()
        {
            var catalogPage = new CatalogPage(_itemService, _userService)
            {
                DataContext = new CatalogViewModel(_itemService, _userService)
            };
            catalogPage.Show();
            ((CatalogViewModel)catalogPage.DataContext).RequestClose += (_, __) => catalogPage.Close();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToProfile()
        {
            if (_userService.IsUserLoggedIn())
            {
                var profilePage = new ProfilePage(_userService, NavigateToMain, NavigateToMain)
                {
                    DataContext = new ProfileViewModel(_userService, NavigateToMain, NavigateToMain)
                };
                ((ProfileViewModel)profilePage.DataContext).RequestClose += (_, __) => profilePage.Close();
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
            var loginWindow = new LoginWindow(_userService, onSuccessfulLogin, onGoBack ?? NavigateToMain)
            {
                DataContext = new LoginViewModel(_userService, onSuccessfulLogin, onGoBack ?? NavigateToMain)
            };
            ((LoginViewModel)loginWindow.DataContext).RequestClose += (_, __) => loginWindow.Close();
            loginWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToAdmin()
        {
            var adminView = new AdminView(_itemService, _userService)
            {
                DataContext = new AdminViewModel(_itemService, _userService, NavigateToMain)
            };
            ((AdminViewModel)adminView.DataContext).RequestClose += (_, __) => adminView.Close();
            adminView.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private bool CanNavigateToAdmin()
        {
            return _userService.GetCurrentUser()?.UserType == UserType.Librarian;
        }

        private void NavigateToMain()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
        }
    }
}

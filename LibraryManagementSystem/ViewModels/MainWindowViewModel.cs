using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System;
using System.Windows;
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
        public IRelayCommand LogoutCommand { get; }

        public event EventHandler RequestClose;

        public MainWindowViewModel(ItemService itemService, UserService userService)
        {
            _itemService = itemService;
            _userService = userService;

            NavigateToCatalogCommand = new RelayCommand(NavigateToCatalog);
            NavigateToProfileCommand = new RelayCommand(NavigateToProfile);
            NavigateToLoginCommand = new RelayCommand(() => NavigateToLogin(NavigateToMain), CanExecuteLogin);
            NavigateToAdminCommand = new RelayCommand(NavigateToAdmin, CanNavigateToAdmin);
            LogoutCommand = new RelayCommand(Logout, CanExecuteLogout);

            UpdateGreetingMessage();
        }

        public string GreetingMessage
        {
            get => _userService.IsUserLoggedIn() ? $"Hello, {_userService.GetCurrentUser().FullName}" : "Hello, Guest";
        }

        public bool IsLoginButtonEnabled
        {
            get => !_userService.IsUserLoggedIn();
        }

        public bool IsLogoutButtonEnabled
        {
            get => _userService.IsUserLoggedIn();
        }

        private bool CanExecuteLogout()
        {
            return _userService.IsUserLoggedIn();
        }

        private bool CanExecuteLogin()
        {
            return !_userService.IsUserLoggedIn();
        }

        private void Logout()
        {
            _userService.Logout();
            UpdateGreetingMessage();
            MessageBox.Show("Logged out successfully.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateGreetingMessage()
        {
            OnPropertyChanged(nameof(GreetingMessage));
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
            OnPropertyChanged(nameof(IsLogoutButtonEnabled));
            LogoutCommand.NotifyCanExecuteChanged();
            NavigateToLoginCommand.NotifyCanExecuteChanged();
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
                var profilePage = new ProfilePage(_userService,_itemService, NavigateToMain, NavigateToMain)
                {
                    DataContext = new ProfileViewModel(_userService, _itemService,NavigateToMain, NavigateToMain)
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
            return _userService.GetCurrentUser()?.UserType == UserType.Manager || _userService.GetCurrentUser()?.UserType == UserType.Librarian;
        }

        private void NavigateToMain()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
        }
    }
}

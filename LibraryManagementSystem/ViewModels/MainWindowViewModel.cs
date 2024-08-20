using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Views;

namespace LibraryManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel for the main window of the library management system, providing navigation functionality to various parts of the application.
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;

        /// <summary>
        /// Command to navigate to the catalog view.
        /// </summary>
        public IRelayCommand NavigateToCatalogCommand { get; }

        /// <summary>
        /// Command to navigate to the user's profile view.
        /// </summary>
        public IRelayCommand NavigateToProfileCommand { get; }

        /// <summary>
        /// Command to navigate to the login view.
        /// </summary>
        public IRelayCommand NavigateToLoginCommand { get; }

        /// <summary>
        /// Command to navigate to the admin view.
        /// </summary>
        public IRelayCommand NavigateToAdminCommand { get; }

        /// <summary>
        /// Command to log out the current user.
        /// </summary>
        public IRelayCommand LogoutCommand { get; }

        /// <summary>
        /// Event triggered when a request to close the current view is made.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="itemService">Service for managing items in the library.</param>
        /// <param name="userService">Service for managing user authentication and information.</param>
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

        /// <summary>
        /// Gets the greeting message based on the current user's login status.
        /// </summary>
        public string GreetingMessage
        {
            get => _userService.IsUserLoggedIn() ? $"Hello, {_userService.GetCurrentUser().FullName}" : "Hello, Guest";
        }

        /// <summary>
        /// Gets a value indicating whether the login button should be enabled.
        /// </summary>
        public bool IsLoginButtonEnabled
        {
            get => !_userService.IsUserLoggedIn();
        }

        /// <summary>
        /// Gets a value indicating whether the logout button should be enabled.
        /// </summary>
        public bool IsLogoutButtonEnabled
        {
            get => _userService.IsUserLoggedIn();
        }

        /// <summary>
        /// Determines whether the logout command can execute.
        /// </summary>
        /// <returns>True if the user is logged in; otherwise, false.</returns>
        private bool CanExecuteLogout()
        {
            return _userService.IsUserLoggedIn();
        }

        /// <summary>
        /// Determines whether the login command can execute.
        /// </summary>
        /// <returns>True if no user is logged in; otherwise, false.</returns>
        private bool CanExecuteLogin()
        {
            return !_userService.IsUserLoggedIn();
        }

        /// <summary>
        /// Logs out the current user and updates the UI.
        /// </summary>
        private void Logout()
        {
            _userService.Logout();
            UpdateGreetingMessage();
            MessageBox.Show("Logged out successfully.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Updates the greeting message and related UI elements based on the current user's login status.
        /// </summary>
        private void UpdateGreetingMessage()
        {
            OnPropertyChanged(nameof(GreetingMessage));
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
            OnPropertyChanged(nameof(IsLogoutButtonEnabled));
            LogoutCommand.NotifyCanExecuteChanged();
            NavigateToLoginCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Navigates to the catalog view where users can browse and search for items.
        /// </summary>
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

        /// <summary>
        /// Navigates to the profile view, allowing the current user to view and manage their profile.
        /// If the user is not logged in, it navigates to the login view first.
        /// </summary>
        private void NavigateToProfile()
        {
            if (_userService.IsUserLoggedIn())
            {
                var profilePage = new ProfilePage(_userService, _itemService, NavigateToMain, NavigateToMain)
                {
                    DataContext = new ProfileViewModel(_userService, _itemService, NavigateToMain, NavigateToMain)
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

        /// <summary>
        /// Navigates to the login view, allowing the user to authenticate.
        /// </summary>
        /// <param name="onSuccessfulLogin">Action to execute after a successful login.</param>
        /// <param name="onGoBack">Action to execute when the user decides to go back to the previous view.</param>
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

        /// <summary>
        /// Navigates to the admin view if the current user has appropriate privileges (Manager or Librarian).
        /// </summary>
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

        /// <summary>
        /// Determines whether the current user has the privileges to navigate to the admin view.
        /// </summary>
        /// <returns>True if the user is a Manager or Librarian; otherwise, false.</returns>
        private bool CanNavigateToAdmin()
        {
            var currentUser = _userService.GetCurrentUser();
            return currentUser?.UserType == UserType.Manager || currentUser?.UserType == UserType.Librarian;
        }

        /// <summary>
        /// Navigates back to the main view.
        /// </summary>
        private void NavigateToMain()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
        }
    }
}

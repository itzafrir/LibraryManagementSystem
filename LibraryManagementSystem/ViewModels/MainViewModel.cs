using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
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
            new Views.CatalogPage { DataContext = new CatalogViewModel(_itemService) }.Show();
        }

        private void NavigateToProfile()
        {
            // Check if the user is logged in
            if (_userService.IsUserLoggedIn())
            {
                new Views.ProfilePage { DataContext = new ProfileViewModel(_userService) }.Show();
            }
            else
            {
                new Views.LoginWindow { DataContext = new LoginViewModel(_userService) }.Show();
            }
        }

        private void NavigateToLogin()
        {
            new Views.LoginWindow { DataContext = new LoginViewModel(_userService) }.Show();
        }
    }
}
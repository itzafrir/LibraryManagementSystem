using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Views;
using System.Windows.Input;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    public partial class CatalogViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private Item _selectedItem;
        private string _searchTerm;
        private ItemType? _selectedItemType;

        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<ItemType> ItemTypes { get; }

        public Item SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public ItemType? SelectedItemType
        {
            get => _selectedItemType;
            set => SetProperty(ref _selectedItemType, value);
        }

        public IRelayCommand SearchCommand { get; }
        public IRelayCommand ViewItemDetailsCommand { get; }
        public IRelayCommand NavigateHomeCommand { get; }
        public IRelayCommand NavigateToLoginCommand { get; }
        public IRelayCommand LogoutCommand { get; }

        public event EventHandler RequestClose;

        public CatalogViewModel(ItemService itemService, UserService userService)
        {
            _itemService = itemService;
            _userService = userService;
            Items = new ObservableCollection<Item>(_itemService.GetAllItems());
            ItemTypes = new ObservableCollection<ItemType>(Enum.GetValues(typeof(ItemType)).Cast<ItemType>());

            SearchCommand = new RelayCommand(SearchItems);
            ViewItemDetailsCommand = new RelayCommand(ViewItemDetails);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin, CanExecuteLogin);
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

        public CatalogViewModelMemento CreateMemento()
        {
            return new CatalogViewModelMemento(SearchTerm, SelectedItemType);
        }

        public void RestoreMemento(CatalogViewModelMemento memento)
        {
            SearchTerm = memento.SearchTerm;
            SelectedItemType = memento.SelectedItemType;
            SearchItems();
        }

        private void SearchItems()
        {
            Items.Clear();
            var searchResults = _itemService.GetAllItems().Where(item =>
                (string.IsNullOrEmpty(SearchTerm) || item.Title.Contains(SearchTerm)) &&
                (!SelectedItemType.HasValue || item.ItemType == SelectedItemType.Value)
            );

            foreach (var item in searchResults)
            {
                Items.Add(item);
            }
        }

        private void ViewItemDetails()
        {
            if (SelectedItem != null)
            {
                var itemDetailPage = new ItemDetailPage(SelectedItem, _itemService, _userService, this)
                {
                    DataContext = new ItemDetailViewModel(SelectedItem, _itemService, _userService, this)
                };
                ((ItemDetailViewModel)itemDetailPage.DataContext).RequestClose += (_, __) => itemDetailPage.Close();
                itemDetailPage.Show();
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
        }

        private void NavigateHome()
        {
            var mainWindow = new Views.MainWindow(_userService, _itemService);
            mainWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateToLogin()
        {
            var memento = CreateMemento();
            var loginWindow = new LoginWindow(_userService, () => GoBackToCatalog(memento), NavigateHome)
            {
                DataContext = new LoginViewModel(_userService, () => GoBackToCatalog(memento), NavigateHome)
            };
            ((LoginViewModel)loginWindow.DataContext).RequestClose += (_, __) => loginWindow.Close();
            loginWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void GoBackToCatalog(CatalogViewModelMemento memento)
        {
            var catalogPage = new CatalogPage(_itemService, _userService)
            {
                DataContext = this
            };
            ((CatalogViewModel)catalogPage.DataContext).RequestClose += (_, __) => catalogPage.Close();
            RestoreMemento(memento);
            catalogPage.Show();
        }
    }
}

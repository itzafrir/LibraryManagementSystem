using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Views;

namespace LibraryManagementSystem.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly Action _navigateHome;

        private FinePayRequest _selectedFinePayRequest;
        public ObservableCollection<FinePayRequest> FinePayRequests { get; }
        public FinePayRequest SelectedFinePayRequest
        {
            get => _selectedFinePayRequest;
            set => SetProperty(ref _selectedFinePayRequest, value);
        }

        public ObservableCollection<Item> Books { get; private set; }
        public ObservableCollection<Item> CDs { get; private set; }
        public ObservableCollection<Item> EBooks { get; private set; }
        public ObservableCollection<Item> DVDs { get; private set; }
        public ObservableCollection<Item> Magazines { get; private set; }
        public ObservableCollection<User> Users { get; private set; }
        public Item SelectedItem { get; set; }
        public User SelectedUser { get; set; }
        public string SearchTerm { get; set; }
        public string SearchUserTerm { get; set; }

        public ICommand SearchCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand SearchUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public ICommand ApproveFinePayRequestCommand { get; }
        public ICommand RejectFinePayRequestCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand LogoutCommand { get; }

        public event EventHandler RequestClose;

        public AdminViewModel(ItemService itemService, UserService userService, Action navigateHome)
        {
            _itemService = itemService;
            _userService = userService;
            _navigateHome = navigateHome;

            Books = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.Book));
            CDs = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.CD));
            EBooks = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.EBook));
            DVDs = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.DVD));
            Magazines = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.Magazine));
            Users = new ObservableCollection<User>(_userService.GetAllUsers());
            FinePayRequests = new ObservableCollection<FinePayRequest>(_userService.GetFinePayRequests());

            SearchCommand = new RelayCommand(SearchItems);
            AddItemCommand = new RelayCommand(OpenAddItemWindow);
            UpdateItemCommand = new RelayCommand(OpenUpdateItemWindow);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            SearchUserCommand = new RelayCommand(SearchUsers);
            AddUserCommand = new RelayCommand(AddUser);
            UpdateUserCommand = new RelayCommand(UpdateUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);

            ApproveFinePayRequestCommand = new RelayCommand(ApproveFinePayRequest);
            RejectFinePayRequestCommand = new RelayCommand(RejectFinePayRequest);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            LogoutCommand = new RelayCommand(Logout, CanExecuteLogout);

            UpdateGreetingMessage();
        }

        private void SearchItems()
        {
            // Clear existing items
            Books.Clear();
            CDs.Clear();
            EBooks.Clear();
            DVDs.Clear();
            Magazines.Clear();

            // Fetch search results
            var searchResults = _itemService.SearchItems(SearchTerm);

            // Add results to appropriate collections
            foreach (var item in searchResults)
            {
                switch (item.ItemType)
                {
                    case ItemType.Book:
                        Books.Add(item);
                        break;
                    case ItemType.CD:
                        CDs.Add(item);
                        break;
                    case ItemType.EBook:
                        EBooks.Add(item);
                        break;
                    case ItemType.DVD:
                        DVDs.Add(item);
                        break;
                    case ItemType.Magazine:
                        Magazines.Add(item);
                        break;
                }
            }
        }

        private void OpenAddItemWindow()
        {
            var addItemWindow = new AddUpdateItemWindow(null, _itemService);
            addItemWindow.ShowDialog();
            RefreshItems();
        }

        private void OpenUpdateItemWindow()
        {
            if (SelectedItem != null)
            {
                var updateItemWindow = new AddUpdateItemWindow(SelectedItem, _itemService);
                updateItemWindow.ShowDialog();
                RefreshItems();
            }
        }

        private void DeleteItem()
        {
            if (SelectedItem != null)
            {
                // Delete item from the database
                _itemService.DeleteItem(SelectedItem.Id);
                // Remove item from the local collection
                switch (SelectedItem.ItemType)
                {
                    case ItemType.Book:
                        Books.Remove(SelectedItem);
                        break;
                    case ItemType.CD:
                        CDs.Remove(SelectedItem);
                        break;
                    case ItemType.EBook:
                        EBooks.Remove(SelectedItem);
                        break;
                    case ItemType.DVD:
                        DVDs.Remove(SelectedItem);
                        break;
                    case ItemType.Magazine:
                        Magazines.Remove(SelectedItem);
                        break;
                }
            }
        }

        private void SearchUsers()
        {
            // Clear existing users
            Users.Clear();

            // Fetch search results
            var searchResults = _userService.SearchUsers(SearchUserTerm);

            // Add results to the local collection
            foreach (var user in searchResults)
            {
                Users.Add(user);
            }
        }

        private void AddUser()
        {
            if (SelectedUser != null)
            {
                // Add user to the database
                _userService.AddUser(SelectedUser);
                // Add user to the local collection
                Users.Add(SelectedUser);
            }
        }

        private void UpdateUser()
        {
            if (SelectedUser != null)
            {
                // Update user in the database
                _userService.UpdateUser(SelectedUser);
                // Refresh the local collection
                SearchUsers();
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                // Delete user from the database
                _userService.DeleteUser(SelectedUser.Id);
                // Remove user from the local collection
                Users.Remove(SelectedUser);
            }
        }

        private void RefreshItems()
        {
            Books = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.Book));
            CDs = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.CD));
            EBooks = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.EBook));
            DVDs = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.DVD));
            Magazines = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.Magazine));
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

        private void Logout()
        {
            _userService.Logout();
            NavigateHome();
        }

        private void UpdateGreetingMessage()
        {
            OnPropertyChanged(nameof(GreetingMessage));
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
            OnPropertyChanged(nameof(IsLogoutButtonEnabled));
        }

        private void ApproveFinePayRequest()
        {
            if (SelectedFinePayRequest != null)
            {
                _userService.ApproveFinePayRequest(SelectedFinePayRequest);
                FinePayRequests.Remove(SelectedFinePayRequest);
            }
        }

        private void RejectFinePayRequest()
        {
            if (SelectedFinePayRequest != null)
            {
                _userService.RejectFinePayRequest(SelectedFinePayRequest);
                FinePayRequests.Remove(SelectedFinePayRequest);
            }
        }

        private void NavigateHome()
        {
            _navigateHome?.Invoke();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}

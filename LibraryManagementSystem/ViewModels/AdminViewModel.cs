using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
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
        private User _selectedUser;
        private Item _selectedItem;
        private string _selectedTab;

        public ObservableCollection<FinePayRequest> FinePayRequests { get; }
        public ObservableCollection<Item> Books { get; private set; }
        public ObservableCollection<Item> CDs { get; private set; }
        public ObservableCollection<Item> EBooks { get; private set; }
        public ObservableCollection<Item> DVDs { get; private set; }
        public ObservableCollection<Item> Magazines { get; private set; }
        public ObservableCollection<User> Users { get; private set; }

        public FinePayRequest SelectedFinePayRequest
        {
            get => _selectedFinePayRequest;
            set => SetProperty(ref _selectedFinePayRequest, value);
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(IsItemSelected));
                ((RelayCommand)UpdateItemCommand).NotifyCanExecuteChanged();
                ((RelayCommand)DeleteItemCommand).NotifyCanExecuteChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                OnPropertyChanged(nameof(IsUserSelected));
                ((RelayCommand)EditUserCommand).NotifyCanExecuteChanged();
                ((RelayCommand)DeleteUserCommand).NotifyCanExecuteChanged();
            }
        }

        public string SelectedTab
        {
            get => ConvertSelectedTab();
            set => SetProperty(ref _selectedTab, value);
        }

        public string SearchTerm { get; set; }
        public string SearchUserTerm { get; set; }

        public bool IsUserSelected => SelectedUser != null;
        public bool IsItemSelected => SelectedItem != null;

        public ICommand SearchCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand SearchUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
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
            AddItemCommand = new RelayCommand(AddItem);
            UpdateItemCommand = new RelayCommand(UpdateItem, () => IsItemSelected);
            DeleteItemCommand = new RelayCommand(DeleteItem, () => IsItemSelected);
            SearchUserCommand = new RelayCommand(SearchUsers);
            AddUserCommand = new RelayCommand(OpenAddUserWindow);
            EditUserCommand = new RelayCommand(OpenEditUserWindow, CanEditOrDeleteUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanEditOrDeleteUser);
            ApproveFinePayRequestCommand = new RelayCommand(ApproveFinePayRequest);
            RejectFinePayRequestCommand = new RelayCommand(RejectFinePayRequest);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            LogoutCommand = new RelayCommand(Logout, CanExecuteLogout);

            UpdateGreetingMessage();
        }

        private void SearchItems()
        {
            Books.Clear();
            CDs.Clear();
            EBooks.Clear();
            DVDs.Clear();
            Magazines.Clear();

            var searchResults = _itemService.SearchItems(SearchTerm);

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

        private void AddItem()
        {
            switch (SelectedTab)
            {
                case "Books":
                    var bookWindow = new BookView { DataContext = new BookViewModel(new Book(), _itemService, () => RefreshItems(ItemType.Book)) };
                    bookWindow.ShowDialog();
                    break;
                case "CDs":
                    var cdWindow = new CDView { DataContext = new CDViewModel(new CD(), _itemService, () => RefreshItems(ItemType.CD)) };
                    cdWindow.ShowDialog();
                    break;
                //case "EBooks":
                //    var eBookWindow = new EBookView { DataContext = new EBookViewModel(new EBook(), _itemService, () => RefreshItems(ItemType.EBook)) };
                //    eBookWindow.ShowDialog();
                //    break;
                //case "DVDs":
                //    var dvdWindow = new DVDView { DataContext = new DVDViewModel(new DVD(), _itemService, () => RefreshItems(ItemType.DVD)) };
                //    dvdWindow.ShowDialog();
                //    break;
                //case "Magazines":
                //    var magazineWindow = new MagazineView { DataContext = new MagazineViewModel(new Magazine(), _itemService, () => RefreshItems(ItemType.Magazine)) };
                //    magazineWindow.ShowDialog();
                //    break;
            }
        }

        private void UpdateItem()
        {
            if (SelectedItem != null)
            {
                switch (SelectedItem)
                {
                    case Book book:
                        var bookWindow = new BookView { DataContext = new BookViewModel(book, _itemService, () => RefreshItems(ItemType.Book)) };
                        bookWindow.ShowDialog();
                        break;
                    case CD cd:
                        var cdWindow = new CDView { DataContext = new CDViewModel(cd, _itemService, () => RefreshItems(ItemType.CD)) };
                        cdWindow.ShowDialog();
                        break;
                    //case EBook eBook:
                    //    var eBookWindow = new EBookView { DataContext = new EBookViewModel(eBook, _itemService, () => RefreshItems(ItemType.EBook)) };
                    //    eBookWindow.ShowDialog();
                    //    break;
                    //case DVD dvd:
                    //    var dvdWindow = new DVDView { DataContext = new DVDViewModel(dvd, _itemService, () => RefreshItems(ItemType.DVD)) };
                    //    dvdWindow.ShowDialog();
                    //    break;
                    //case Magazine magazine:
                    //    var magazineWindow = new MagazineView { DataContext = new MagazineViewModel(magazine, _itemService, () => RefreshItems(ItemType.Magazine)) };
                    //    magazineWindow.ShowDialog();
                    //    break;
                }
            }
        }

        private void DeleteItem()
        {
            if (SelectedItem != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _itemService.DeleteItem(SelectedItem.Id);

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
        }

        private void SearchUsers()
        {
            Users.Clear();

            var searchResults = _userService.SearchUsers(SearchUserTerm);

            foreach (var user in searchResults)
            {
                Users.Add(user);
            }
        }

        private void OpenAddUserWindow()
        {
            var addUserWindow = new AddUpdateUserWindow(new User(), _userService);
            addUserWindow.ShowDialog();
            RefreshUsers();
        }

        private void OpenEditUserWindow()
        {
            if (SelectedUser != null)
            {
                var editUserWindow = new AddUpdateUserWindow(SelectedUser, _userService);
                editUserWindow.ShowDialog();
                RefreshUsers();
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _userService.DeleteUser(SelectedUser.Id);
                        Users.Remove(SelectedUser);
                        SelectedUser = null; // Unselect the user after deletion
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void RefreshItems(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Book:
                    Books.Clear();
                    foreach (var book in _itemService.GetItemsByType(ItemType.Book))
                    {
                        Books.Add(book);
                    }
                    break;
                case ItemType.CD:
                    CDs.Clear();
                    foreach (var cd in _itemService.GetItemsByType(ItemType.CD))
                    {
                        CDs.Add(cd);
                    }
                    break;
                case ItemType.EBook:
                    EBooks.Clear();
                    foreach (var ebook in _itemService.GetItemsByType(ItemType.EBook))
                    {
                        EBooks.Add(ebook);
                    }
                    break;
                case ItemType.DVD:
                    DVDs.Clear();
                    foreach (var dvd in _itemService.GetItemsByType(ItemType.DVD))
                    {
                        DVDs.Add(dvd);
                    }
                    break;
                case ItemType.Magazine:
                    Magazines.Clear();
                    foreach (var magazine in _itemService.GetItemsByType(ItemType.Magazine))
                    {
                        Magazines.Add(magazine);
                    }
                    break;
            }
        }

        private void RefreshUsers()
        {
            Users.Clear();
            foreach (var user in _userService.GetAllUsers())
            {
                Users.Add(user);
            }
        }

        private bool CanEditOrDeleteUser()
        {
            return SelectedUser != null;
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
        private string ConvertSelectedTab()
        {
            if (_selectedTab == null)
            {
                return "";
            }
            if (_selectedTab.Contains("Books") && !_selectedTab.Contains("EBooks") )
            {
                return "Books";
            }
            if (_selectedTab.Contains("CDs"))
            {
                return "CDs";
            }
            if (_selectedTab.Contains("DVD"))
            {
                return "DVD";
            }
            if (_selectedTab.Contains("EBooks"))
            {
                return "EBooks";
            }
            if (_selectedTab.Contains("Magazine"))
            {
                return "Magazine";
            }

            return "";
        }
    }
}

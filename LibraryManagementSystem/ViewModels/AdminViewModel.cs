using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel for the admin interface, providing functionality for managing items, users, and fines in the library system.
    /// </summary>
    public partial class AdminViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly Action _navigateHome;

        private FinePayRequest _selectedFinePayRequest;
        private User _selectedUser;
        private Item _selectedItem;

        /// <summary>
        /// Gets or sets the collection of fine payment requests.
        /// </summary>
        public ObservableCollection<FinePayRequest> FinePayRequests { get; private set; }

        /// <summary>
        /// Gets or sets the collection of books in the system.
        /// </summary>
        public ObservableCollection<Book> Books { get; private set; }

        /// <summary>
        /// Gets or sets the collection of CDs in the system.
        /// </summary>
        public ObservableCollection<CD> CDs { get; private set; }

        /// <summary>
        /// Gets or sets the collection of eBooks in the system.
        /// </summary>
        public ObservableCollection<EBook> EBooks { get; private set; }

        /// <summary>
        /// Gets or sets the collection of DVDs in the system.
        /// </summary>
        public ObservableCollection<DVD> DVDs { get; private set; }

        /// <summary>
        /// Gets or sets the collection of magazines in the system.
        /// </summary>
        public ObservableCollection<Magazine> Magazines { get; private set; }

        /// <summary>
        /// Gets or sets the collection of users in the system.
        /// </summary>
        public ObservableCollection<User> Users { get; private set; }

        /// <summary>
        /// Gets or sets the currently selected fine payment request.
        /// </summary>
        public FinePayRequest SelectedFinePayRequest
        {
            get => _selectedFinePayRequest;
            set => SetProperty(ref _selectedFinePayRequest, value);
        }

        /// <summary>
        /// Gets or sets the currently selected item.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the currently selected user.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the search term for items.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the search term for users.
        /// </summary>
        public string SearchUserTerm { get; set; }

        /// <summary>
        /// Gets a value indicating whether a user is selected.
        /// </summary>
        public bool IsUserSelected => SelectedUser != null;

        /// <summary>
        /// Gets a value indicating whether an item is selected.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminViewModel"/> class.
        /// </summary>
        /// <param name="itemService">The service used to manage items.</param>
        /// <param name="userService">The service used to manage users.</param>
        /// <param name="navigateHome">The action to navigate back to the home screen.</param>
        public AdminViewModel(ItemService itemService, UserService userService, Action navigateHome)
        {
            _itemService = itemService;
            _userService = userService;
            _navigateHome = navigateHome;

            // Initialize collections
            Books = new ObservableCollection<Book>(_itemService.GetItemsByType<Book>());
            CDs = new ObservableCollection<CD>(_itemService.GetItemsByType<CD>());
            EBooks = new ObservableCollection<EBook>(_itemService.GetItemsByType<EBook>());
            DVDs = new ObservableCollection<DVD>(_itemService.GetItemsByType<DVD>());
            Magazines = new ObservableCollection<Magazine>(_itemService.GetItemsByType<Magazine>());
            Users = new ObservableCollection<User>(_userService.GetAllUsers());
            FinePayRequests = new ObservableCollection<FinePayRequest>(_userService.GetFinePayRequests());

            // Define commands
            SearchCommand = new RelayCommand(SearchItems);
            AddItemCommand = new RelayCommand(OpenAddItemWindow);
            UpdateItemCommand = new RelayCommand(OpenUpdateItemWindow, () => IsItemSelected);
            DeleteItemCommand = new RelayCommand(DeleteItem, () => IsItemSelected);

            SearchUserCommand = new RelayCommand(SearchUsers);
            AddUserCommand = new RelayCommand(OpenAddUserWindow, CanManageUsers);
            EditUserCommand = new RelayCommand(OpenEditUserWindow, () => IsUserSelected && CanManageUsers());
            DeleteUserCommand = new RelayCommand(DeleteUser, () => IsUserSelected && CanManageUsers());

            ApproveFinePayRequestCommand = new RelayCommand(ApproveFinePayRequest, CanManageFines);
            RejectFinePayRequestCommand = new RelayCommand(RejectFinePayRequest, CanManageFines);

            NavigateHomeCommand = new RelayCommand(NavigateHome);
            LogoutCommand = new RelayCommand(Logout, CanExecuteLogout);

            UpdateGreetingMessage();
        }

        /// <summary>
        /// Determines whether the current user can manage users.
        /// </summary>
        /// <returns><c>true</c> if the current user is a manager; otherwise, <c>false</c>.</returns>
        private bool CanManageUsers()
        {
            return _userService.GetCurrentUser().UserType == UserType.Manager;
        }

        /// <summary>
        /// Determines whether the current user can manage fines.
        /// </summary>
        /// <returns><c>true</c> if the current user is a manager; otherwise, <c>false</c>.</returns>
        private bool CanManageFines()
        {
            return _userService.GetCurrentUser().UserType == UserType.Manager;
        }

        /// <summary>
        /// Searches for items based on the search term.
        /// </summary>
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
                switch (item)
                {
                    case Book book:
                        Books.Add(book);
                        break;
                    case CD cd:
                        CDs.Add(cd);
                        break;
                    case EBook ebook:
                        EBooks.Add(ebook);
                        break;
                    case DVD dvd:
                        DVDs.Add(dvd);
                        break;
                    case Magazine magazine:
                        Magazines.Add(magazine);
                        break;
                }
            }
        }

        /// <summary>
        /// Opens the window to add a new item.
        /// </summary>
        private void OpenAddItemWindow()
        {
            var itemTypeSelectionWindow = new ItemTypeSelectionWindow();
            if (itemTypeSelectionWindow.ShowDialog() == true)
            {
                var selectedItemType = itemTypeSelectionWindow.SelectedItemType;
                Window addItemWindow = null;

                switch (selectedItemType)
                {
                    case ItemType.Book:
                        addItemWindow = new BookView(new Book(), _itemService);
                        break;
                    case ItemType.CD:
                        addItemWindow = new CDView(new CD(), _itemService);
                        break;
                    case ItemType.EBook:
                        addItemWindow = new EBookView(new EBook(), _itemService);
                        break;
                    case ItemType.DVD:
                        addItemWindow = new DVDView(new DVD(), _itemService);
                        break;
                    case ItemType.Magazine:
                        addItemWindow = new MagazineView(new Magazine(), _itemService);
                        break;
                }

                if (addItemWindow != null)
                {
                    addItemWindow.ShowDialog();
                    RefreshItems();
                }
            }
        }

        /// <summary>
        /// Opens the window to update the selected item.
        /// </summary>
        private void OpenUpdateItemWindow()
        {
            if (SelectedItem != null)
            {
                Window updateItemWindow = null;
                switch (SelectedItem)
                {
                    case Book book:
                        updateItemWindow = new BookView(book, _itemService);
                        break;
                    case CD cd:
                        updateItemWindow = new CDView(cd, _itemService);
                        break;
                    case EBook ebook:
                        updateItemWindow = new EBookView(ebook, _itemService);
                        break;
                    case DVD dvd:
                        updateItemWindow = new DVDView(dvd, _itemService);
                        break;
                    case Magazine magazine:
                        updateItemWindow = new MagazineView(magazine, _itemService);
                        break;
                }

                if (updateItemWindow != null)
                {
                    updateItemWindow.ShowDialog();
                    RefreshItems();
                }
            }
        }

        /// <summary>
        /// Refreshes the list of items displayed in the UI.
        /// </summary>
        private void RefreshItems()
        {
            Books.Clear();
            foreach (var book in _itemService.GetItemsByType<Book>())
            {
                Books.Add(book);
            }
            CDs.Clear();
            foreach (var cd in _itemService.GetItemsByType<CD>())
            {
                CDs.Add(cd);
            }
            EBooks.Clear();
            foreach (var ebook in _itemService.GetItemsByType<EBook>())
            {
                EBooks.Add(ebook);
            }
            DVDs.Clear();
            foreach (var dvd in _itemService.GetItemsByType<DVD>())
            {
                DVDs.Add(dvd);
            }
            Magazines.Clear();
            foreach (var magazine in _itemService.GetItemsByType<Magazine>())
            {
                Magazines.Add(magazine);
            }
        }

        /// <summary>
        /// Deletes the selected item after confirming with the user.
        /// </summary>
        private void DeleteItem()
        {
            if (SelectedItem != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    bool isDeleted = _itemService.DeleteItem(SelectedItem.Id);

                    if (isDeleted)
                    {
                        switch (SelectedItem)
                        {
                            case Book book:
                                Books.Remove(book);
                                break;
                            case CD cd:
                                CDs.Remove(cd);
                                break;
                            case EBook ebook:
                                EBooks.Remove(ebook);
                                break;
                            case DVD dvd:
                                DVDs.Remove(dvd);
                                break;
                            case Magazine magazine:
                                Magazines.Remove(magazine);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item deletion was canceled due to active loans or pending loan requests.", "Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        /// <summary>
        /// Searches for users based on the search term.
        /// </summary>
        private void SearchUsers()
        {
            Users.Clear();

            var searchResults = _userService.SearchUsers(SearchUserTerm);

            foreach (var user in searchResults)
            {
                Users.Add(user);
            }
        }

        /// <summary>
        /// Opens the window to add a new user.
        /// </summary>
        private void OpenAddUserWindow()
        {
            var addUserWindow = new AddUpdateUserWindow(null, _userService);
            addUserWindow.ShowDialog();
            RefreshUsers();
        }

        /// <summary>
        /// Opens the window to edit the selected user.
        /// </summary>
        private void OpenEditUserWindow()
        {
            if (SelectedUser != null)
            {
                var editUserWindow = new AddUpdateUserWindow(SelectedUser, _userService);
                editUserWindow.ShowDialog();
                RefreshUsers();
            }
        }

        /// <summary>
        /// Deletes the selected user after confirming with the user.
        /// </summary>
        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (SelectedUser == _userService.GetCurrentUser())
                    {
                        MessageBox.Show("Cannot delete current user", "Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    bool isDeleted = _userService.DeleteUser(SelectedUser.Id);

                    if (isDeleted)
                    {
                        Users.Remove(SelectedUser);
                        SelectedUser = null;
                    }
                    else
                    {
                        MessageBox.Show("User deletion was canceled due to active loans, loan requests, or outstanding fines.", "Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes the list of users displayed in the UI.
        /// </summary>
        private void RefreshUsers()
        {
            Users.Clear();
            foreach (var user in _userService.GetAllUsers())
            {
                Users.Add(user);
            }
        }

        /// <summary>
        /// Logs out the current user and navigates back to the home screen.
        /// </summary>
        private void Logout()
        {
            _userService.Logout();
            NavigateHome();
        }

        /// <summary>
        /// Updates the greeting message displayed in the UI based on the current user.
        /// </summary>
        private void UpdateGreetingMessage()
        {
            OnPropertyChanged(nameof(GreetingMessage));
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
            OnPropertyChanged(nameof(IsLogoutButtonEnabled));
        }

        /// <summary>
        /// Approves the selected fine payment request.
        /// </summary>
        private void ApproveFinePayRequest()
        {
            if (SelectedFinePayRequest != null)
            {
                var approvalResult = _userService.ApproveFinePayRequest(SelectedFinePayRequest);

                if (approvalResult == "Success")
                {
                    FinePayRequests.Remove(SelectedFinePayRequest);
                    MessageBox.Show("Fine payment approved successfully.", "Approval Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(approvalResult, "Approval Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Rejects the selected fine payment request.
        /// </summary>
        private void RejectFinePayRequest()
        {
            if (SelectedFinePayRequest != null)
            {
                _userService.RejectFinePayRequest(SelectedFinePayRequest);
                FinePayRequests.Remove(SelectedFinePayRequest);
                MessageBox.Show("Fine payment rejected successfully.", "Rejection Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Navigates back to the home screen.
        /// </summary>
        private void NavigateHome()
        {
            _navigateHome?.Invoke();
            RequestClose?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Determines whether the logout command can execute.
        /// </summary>
        /// <returns><c>true</c> if a user is logged in; otherwise, <c>false</c>.</returns>
        private bool CanExecuteLogout()
        {
            return _userService.IsUserLoggedIn();
        }
        private bool CanEditOrDeleteUser()
        {
            return SelectedUser != null;
        }
    }
}

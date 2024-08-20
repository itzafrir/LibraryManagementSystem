using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel for managing the catalog view, allowing users to search and view details of items in the library.
    /// </summary>
    public partial class CatalogViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private Item _selectedItem;
        private string _searchTerm;
        private ItemType? _selectedItemType;

        /// <summary>
        /// Gets the collection of items currently displayed in the catalog.
        /// </summary>
        public ObservableCollection<Item> Items { get; }

        /// <summary>
        /// Gets the collection of available item types for filtering.
        /// </summary>
        public ObservableCollection<ItemType> ItemTypes { get; }

        /// <summary>
        /// Gets or sets the currently selected item.
        /// </summary>
        public Item SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        /// <summary>
        /// Gets or sets the search term entered by the user.
        /// </summary>
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        /// <summary>
        /// Gets or sets the selected item type for filtering the catalog.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogViewModel"/> class.
        /// </summary>
        /// <param name="itemService">The service used to manage items.</param>
        /// <param name="userService">The service used to manage users.</param>
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

        /// <summary>
        /// Gets the greeting message for the current user.
        /// </summary>
        public string GreetingMessage
        {
            get => _userService.IsUserLoggedIn() ? $"Hello, {_userService.GetCurrentUser().FullName}" : "Hello, Guest";
        }

        /// <summary>
        /// Gets a value indicating whether the login button is enabled.
        /// </summary>
        public bool IsLoginButtonEnabled
        {
            get => !_userService.IsUserLoggedIn();
        }

        /// <summary>
        /// Gets a value indicating whether the logout button is enabled.
        /// </summary>
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

        /// <summary>
        /// Determines whether the login command can execute.
        /// </summary>
        /// <returns><c>true</c> if no user is logged in; otherwise, <c>false</c>.</returns>
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
        /// Updates the greeting message and UI elements related to user login status.
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
        /// Creates a memento for saving the current state of the view model.
        /// </summary>
        /// <returns>A <see cref="CatalogViewModelMemento"/> representing the current state.</returns>
        public CatalogViewModelMemento CreateMemento()
        {
            return new CatalogViewModelMemento(SearchTerm, SelectedItemType);
        }

        /// <summary>
        /// Restores the view model state from a memento.
        /// </summary>
        /// <param name="memento">The memento containing the saved state.</param>
        public void RestoreMemento(CatalogViewModelMemento memento)
        {
            SearchTerm = memento.SearchTerm;
            SelectedItemType = memento.SelectedItemType;
            SearchItems();
        }

        /// <summary>
        /// Searches for items in the catalog based on the search term and selected item type.
        /// </summary>
        private void SearchItems()
        {
            Items.Clear();
            var searchResults = _itemService.GetAllItems().Where(item =>
                (string.IsNullOrEmpty(SearchTerm) || MatchesSearchTerm(item)) &&
                (!SelectedItemType.HasValue || (item.GetItemTypeName() == SelectedItemType.Value.ToString() || SelectedItemType.Value == ItemType.All))
            );

            foreach (var item in searchResults)
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// Checks if an item matches the search term.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns><c>true</c> if the item matches the search term; otherwise, <c>false</c>.</returns>
        private bool MatchesSearchTerm(Item item)
        {
            if (item.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                item.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return item switch
            {
                Book book => book.Author.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             book.Genre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             book.Language.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             book.Format.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                             book.Series.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase),

                CD cd => cd.Artist.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                         cd.Genre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                         cd.Label.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase),

                DVD dvd => dvd.Director.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           dvd.Genre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           dvd.Language.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           dvd.Studio.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase),

                EBook ebook => ebook.Author.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                               ebook.FileFormat.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase),

                Magazine magazine => magazine.Editor.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     magazine.Genre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     magazine.Frequency.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase),

                _ => false,
            };
        }

        /// <summary>
        /// Opens the item details page for the selected item.
        /// </summary>
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

        /// <summary>
        /// Navigates back to the home screen.
        /// </summary>
        private void NavigateHome()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Navigates to the login screen.
        /// </summary>
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

        /// <summary>
        /// Restores the catalog view from a memento and displays it.
        /// </summary>
        /// <param name="memento">The memento containing the saved state.</param>
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

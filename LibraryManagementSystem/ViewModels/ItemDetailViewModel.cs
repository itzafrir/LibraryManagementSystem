using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel responsible for handling the logic and data binding for the item detail view.
    /// Allows users to view detailed information about a selected item, add reviews, and loan items.
    /// </summary>
    public partial class ItemDetailViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly CatalogViewModel _catalogViewModel;
        private readonly CatalogViewModelMemento _catalogViewModelMemento;

        private string _newReviewText;
        private int _newReviewRating;
        private bool _canAddReview;

        /// <summary>
        /// Gets the currently selected item whose details are being displayed.
        /// </summary>
        public Item SelectedItem { get; private set; }

        /// <summary>
        /// Command to initiate the loan process for the selected item.
        /// </summary>
        public ICommand LoanItemCommand { get; }

        /// <summary>
        /// Command to navigate back to the home view.
        /// </summary>
        public ICommand NavigateHomeCommand { get; }

        /// <summary>
        /// Command to navigate back to the catalog view.
        /// </summary>
        public ICommand GoBackToCatalogCommand { get; }

        /// <summary>
        /// Command to navigate to the login view.
        /// </summary>
        public IRelayCommand NavigateToLoginCommand { get; }

        /// <summary>
        /// Command to add a new review for the selected item.
        /// </summary>
        public IRelayCommand AddReviewCommand { get; }

        /// <summary>
        /// Command to log out the current user.
        /// </summary>
        public IRelayCommand LogoutCommand { get; }

        /// <summary>
        /// Event triggered when a request to close the current view is made.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDetailViewModel"/> class.
        /// </summary>
        /// <param name="selectedItem">The item whose details are to be displayed.</param>
        /// <param name="itemService">Service for managing items.</param>
        /// <param name="userService">Service for managing user authentication and information.</param>
        /// <param name="catalogViewModel">The catalog view model to return to after operations.</param>
        public ItemDetailViewModel(Item selectedItem, ItemService itemService, UserService userService, CatalogViewModel catalogViewModel)
        {
            SelectedItem = selectedItem;
            _itemService = itemService;
            _userService = userService;
            _catalogViewModel = catalogViewModel;
            _catalogViewModelMemento = _catalogViewModel.CreateMemento();

            LoanItemCommand = new RelayCommand(LoanItem);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            GoBackToCatalogCommand = new RelayCommand(GoBackToCatalog);
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
            AddReviewCommand = new RelayCommand(AddReview, CanExecuteAddReview);
            LogoutCommand = new RelayCommand(Logout, CanExecuteLogout);

            UpdateCanAddReview();
            UpdateGreetingMessage();
        }

        /// <summary>
        /// Gets or sets the text for the new review being added.
        /// </summary>
        public string NewReviewText
        {
            get => _newReviewText;
            set
            {
                SetProperty(ref _newReviewText, value);
                UpdateCanAddReview();
            }
        }

        /// <summary>
        /// Gets or sets the rating for the new review being added.
        /// </summary>
        public int NewReviewRating
        {
            get => _newReviewRating;
            set
            {
                SetProperty(ref _newReviewRating, value);
                UpdateCanAddReview();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can add a new review.
        /// </summary>
        public bool CanAddReview
        {
            get => _canAddReview;
            private set => SetProperty(ref _canAddReview, value);
        }

        /// <summary>
        /// Gets the formatted string displaying average rating and total number of reviews for the selected item.
        /// </summary>
        public string RatingAndReviewCount => $"{SelectedItem.AverageRating:F1} ({SelectedItem.Reviews.Count} reviews)";

        /// <summary>
        /// Gets the creator (author, artist, etc.) of the selected item.
        /// </summary>
        public string SelectedCreator => SelectedItem?.GetCreator();

        /// <summary>
        /// Gets the greeting message based on the current user's login status.
        /// </summary>
        public string GreetingMessage => _userService.IsUserLoggedIn() ? $"Hello, {_userService.GetCurrentUser().FullName}" : "Hello, Guest";

        /// <summary>
        /// Gets a value indicating whether the login button should be enabled.
        /// </summary>
        public bool IsLoginButtonEnabled => !_userService.IsUserLoggedIn();

        /// <summary>
        /// Gets a value indicating whether the logout button should be enabled.
        /// </summary>
        public bool IsLogoutButtonEnabled => _userService.IsUserLoggedIn();

        /// <summary>
        /// Determines whether the add review command can execute.
        /// </summary>
        /// <returns>True if the user is logged in and has not already reviewed the item; otherwise, false.</returns>
        private bool CanExecuteAddReview()
        {
            var currentUser = _userService.GetCurrentUser();
            return currentUser != null && !SelectedItem.Reviews.Any(r => r.UserId == currentUser.Id);
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
        /// Updates the ability to add a review based on the current state.
        /// </summary>
        private void UpdateCanAddReview()
        {
            CanAddReview = CanExecuteAddReview() && !string.IsNullOrWhiteSpace(NewReviewText) && NewReviewRating > 0;
            AddReviewCommand.NotifyCanExecuteChanged();
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
        /// Adds a new review to the selected item.
        /// </summary>
        private void AddReview()
        {
            if (_userService.IsUserLoggedIn() && !string.IsNullOrWhiteSpace(NewReviewText) && NewReviewRating > 0)
            {
                try
                {
                    var currentUser = _userService.GetCurrentUser();
                    var review = new Review(currentUser.Id, SelectedItem.Id, NewReviewRating, NewReviewText);
                    _itemService.AddReview(SelectedItem, review);

                    // Update UI after successful addition
                    OnPropertyChanged(nameof(SelectedItem.Reviews));
                    OnPropertyChanged(nameof(SelectedItem.AverageRating));
                    OnPropertyChanged(nameof(RatingAndReviewCount));

                    // Reset review input fields
                    NewReviewText = string.Empty;
                    NewReviewRating = 0;
                    UpdateCanAddReview();

                    MessageBox.Show("Review added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please provide both a rating and review text.", "Incomplete Review", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Initiates the loan process for the selected item.
        /// </summary>
        private void LoanItem()
        {
            try
            {
                var currentUser = _userService.GetCurrentUser();
                if (currentUser == null)
                {
                    MessageBox.Show("You must be logged in to loan an item.", "Authentication Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (currentUser.CurrentLoans.Any(l => l.ItemId == SelectedItem.Id && l.LoanStatus == LoanStatus.Active))
                {
                    MessageBox.Show("You already have an active loan for this item.", "Duplicate Loan", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _itemService.LoanItem(currentUser, SelectedItem);
                MessageBox.Show("Item loaned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                OnPropertyChanged(nameof(SelectedItem));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Loan Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Creates a memento object to save the current state of the item detail view.
        /// </summary>
        /// <returns>A memento containing the current selected item.</returns>
        public ItemDetailViewModelMemento CreateMemento()
        {
            return new ItemDetailViewModelMemento(SelectedItem);
        }

        /// <summary>
        /// Restores the state of the item detail view from a given memento.
        /// </summary>
        /// <param name="memento">The memento containing the state to restore.</param>
        public void RestoreMemento(ItemDetailViewModelMemento memento)
        {
            SelectedItem = memento.SelectedItem;
            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(RatingAndReviewCount));
            OnPropertyChanged(nameof(SelectedCreator));
        }

        /// <summary>
        /// Navigates back to the home view.
        /// </summary>
        private void NavigateHome()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Navigates back to the catalog view, restoring its previous state.
        /// </summary>
        private void GoBackToCatalog()
        {
            var catalogPage = new CatalogPage(_itemService, _userService)
            {
                DataContext = _catalogViewModel
            };
            ((CatalogViewModel)catalogPage.DataContext).RequestClose += (_, __) => catalogPage.Close();
            _catalogViewModel.RestoreMemento(_catalogViewModelMemento);
            catalogPage.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Navigates to the login view, allowing the user to authenticate.
        /// </summary>
        private void NavigateToLogin()
        {
            var memento = CreateMemento();
            var loginWindow = new LoginWindow(_userService, () => GoBackToItemDetail(memento), NavigateHome)
            {
                DataContext = new LoginViewModel(_userService, () => GoBackToItemDetail(memento), NavigateHome)
            };
            ((LoginViewModel)loginWindow.DataContext).RequestClose += (_, __) => loginWindow.Close();
            loginWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Navigates back to the item detail view after a successful login, restoring its previous state.
        /// </summary>
        /// <param name="memento">The memento containing the state to restore.</param>
        private void GoBackToItemDetail(ItemDetailViewModelMemento memento)
        {
            var itemDetailPage = new ItemDetailPage(memento.SelectedItem, _itemService, _userService, _catalogViewModel)
            {
                DataContext = this
            };
            ((ItemDetailViewModel)itemDetailPage.DataContext).RequestClose += (_, __) => itemDetailPage.Close();
            RestoreMemento(memento);
            itemDetailPage.Show();
        }

        /// <summary>
        /// Logs out the current user and updates the UI accordingly.
        /// </summary>
        private void Logout()
        {
            _userService.Logout();
            UpdateGreetingMessage();
            MessageBox.Show("Logged out successfully.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

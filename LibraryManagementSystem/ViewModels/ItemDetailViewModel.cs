using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.Views;

namespace LibraryManagementSystem.ViewModels
{
    public partial class ItemDetailViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly CatalogViewModel _catalogViewModel;
        private readonly CatalogViewModelMemento _catalogViewModelMemento;

        private string _newReviewText;
        private int _newReviewRating;
        private bool _canAddReview;

        public Item SelectedItem { get; private set; }

        public ICommand LoanItemCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand GoBackToCatalogCommand { get; }
        public IRelayCommand NavigateToLoginCommand { get; }
        public IRelayCommand AddReviewCommand { get; }
        public IRelayCommand LogoutCommand { get; }

        public event EventHandler RequestClose;

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

        public string NewReviewText
        {
            get => _newReviewText;
            set
            {
                SetProperty(ref _newReviewText, value);
                UpdateCanAddReview();
            }
        }

        public int NewReviewRating
        {
            get => _newReviewRating;
            set
            {
                SetProperty(ref _newReviewRating, value);
                UpdateCanAddReview();
            }
        }

        public bool CanAddReview
        {
            get => _canAddReview;
            private set => SetProperty(ref _canAddReview, value);
        }

        public string RatingAndReviewCount
        {
            get => $"{SelectedItem.AverageRating:F1} ({SelectedItem.Reviews.Count} reviews)";
        }

        public string GreetingMessage => _userService.IsUserLoggedIn() ? $"Hello, {_userService.GetCurrentUser().FullName}" : "Hello, Guest";

        public bool IsLoginButtonEnabled => !_userService.IsUserLoggedIn();
        public bool IsLogoutButtonEnabled => _userService.IsUserLoggedIn();

        private bool CanExecuteAddReview()
        {
            var currentUser = _userService.GetCurrentUser();
            return currentUser != null && !SelectedItem.Reviews.Any(r => r.UserId == currentUser.Id);
        }

        private bool CanExecuteLogout()
        {
            return _userService.IsUserLoggedIn();
        }

        private void UpdateCanAddReview()
        {
            CanAddReview = CanExecuteAddReview();
            AddReviewCommand.NotifyCanExecuteChanged();
        }

        private void UpdateGreetingMessage()
        {
            OnPropertyChanged(nameof(GreetingMessage));
            OnPropertyChanged(nameof(IsLoginButtonEnabled));
            OnPropertyChanged(nameof(IsLogoutButtonEnabled));
        }

        private void AddReview()
        {
            if (_userService.IsUserLoggedIn() && !string.IsNullOrWhiteSpace(NewReviewText) && NewReviewRating > 0)
            {
                try
                {
                    var review = new Review(_userService.GetCurrentUser().Id, SelectedItem.Id, NewReviewRating, NewReviewText);
                    _itemService.AddReview(SelectedItem, review); // This will now check for duplicates

                    // Only update the UI if the review was successfully added
                    OnPropertyChanged(nameof(SelectedItem.Reviews));
                    OnPropertyChanged(nameof(SelectedItem.AverageRating));
                    OnPropertyChanged(nameof(RatingAndReviewCount));
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
                MessageBox.Show("Please provide a rating and review text.");
            }
        }

        private void LoanItem()
        {
            try
            {
                _itemService.LoanItem(_userService.GetCurrentUser(), SelectedItem);
                OnPropertyChanged(nameof(SelectedItem));
                MessageBox.Show("Item loaned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ItemDetailViewModelMemento CreateMemento()
        {
            return new ItemDetailViewModelMemento(SelectedItem);
        }

        public void RestoreMemento(ItemDetailViewModelMemento memento)
        {
            SelectedItem = memento.SelectedItem;
            OnPropertyChanged(nameof(SelectedItem));
        }

        private void NavigateHome()
        {
            var mainWindow = new MainWindow(_userService, _itemService);
            mainWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

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

        private void Logout()
        {
            _userService.Logout();
            UpdateGreetingMessage();
            MessageBox.Show("Logged out successfully.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

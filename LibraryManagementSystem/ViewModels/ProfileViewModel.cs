using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly Action _onGoBack;
        private User _userProfile;

        public User UserProfile
        {
            get => _userProfile;
            private set => SetProperty(ref _userProfile, value);
        }

        public ObservableCollection<Loan> CurrentLoans { get; }
        public ObservableCollection<Item> Orders { get; }
        public ObservableCollection<Fine> Fines { get; }

        public ICommand LogoutCommand { get; }
        public ICommand GoBackCommand { get; }

        public event EventHandler RequestClose;

        public ProfileViewModel(UserService userService, Action onGoBack)
        {
            _userService = userService;
            _onGoBack = onGoBack;

            UserProfile = _userService.GetCurrentUser();
            CurrentLoans = new ObservableCollection<Loan>(_userService.GetCurrentLoans());
            Orders = new ObservableCollection<Item>(_userService.GetOrders());
            Fines = new ObservableCollection<Fine>(_userService.GetFines());

            LogoutCommand = new RelayCommand(Logout);
            GoBackCommand = new RelayCommand(GoBack);
        }

        private void Logout()
        {
            _userService.Logout();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void GoBack()
        {
            _onGoBack?.Invoke();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
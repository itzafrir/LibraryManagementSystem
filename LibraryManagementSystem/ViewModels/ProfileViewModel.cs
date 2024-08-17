using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Views;

public partial class ProfileViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly ItemService _itemService;
    private readonly Action _onGoBack;
    private readonly Action _onLogout;
    private User _userProfile;

    public User UserProfile
    {
        get => _userProfile;
        private set => SetProperty(ref _userProfile, value);
    }

    public ObservableCollection<Loan> CurrentLoans { get; }
    public ObservableCollection<LoanRequest> LoanRequests { get; }
    public ObservableCollection<Fine> Fines { get; }

    public ICommand LogoutCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand PayFineCommand { get; }
    public ICommand ReturnItemCommand { get; }
    public ICommand UpdateProfileCommand { get; }

    public event EventHandler RequestClose;

    public ProfileViewModel(UserService userService, ItemService itemService, Action onGoBack, Action onLogout)
    {
        _userService = userService;
        _itemService = itemService;
        _onGoBack = onGoBack;
        _onLogout = onLogout;

        UserProfile = _userService.GetCurrentUser();
        CurrentLoans = new ObservableCollection<Loan>(_userService.GetCurrentLoans());
        LoanRequests = new ObservableCollection<LoanRequest>(_userService.GetLoanRequests());
        Fines = new ObservableCollection<Fine>(_userService.GetFines());

        LogoutCommand = new RelayCommand(Logout);
        GoBackCommand = new RelayCommand(GoBack);
        PayFineCommand = new RelayCommand<Fine>(PayFine);
        ReturnItemCommand = new RelayCommand<Loan>(ReturnItem);

        // Initialize the UpdateProfileCommand
        UpdateProfileCommand = new RelayCommand(UpdateProfile);
    }

    private void UpdateProfile()
    {
        var updateUserWindow = new AddUpdateUserWindow(UserProfile, _userService, isEditable: false);
        updateUserWindow.ShowDialog();
    }
    private void Logout()
    {
        _userService.Logout();
        _onLogout?.Invoke();
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void GoBack()
    {
        _onGoBack?.Invoke();
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void PayFine(Fine fine)
    {
        if (fine != null)
        {
            _userService.CreateFinePayRequest(fine);
            Fines.Remove(fine);
        }
    }

    private void ReturnItem(Loan loan)
    {
        if (loan != null)
        {
            // Delegate the loan return process to ItemService
            _itemService.ReturnLoan(loan);
            CurrentLoans.Remove(loan);

            MessageBox.Show("Item returned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

public partial class ProfileViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly Action _onGoBack;
    private readonly Action _onLogout;
    private User _userProfile;

    public User UserProfile
    {
        get => _userProfile;
        private set => SetProperty(ref _userProfile, value);
    }

    public ObservableCollection<Loan> CurrentLoans { get; private set; }
    public ObservableCollection<LoanRequest> LoanRequests { get; private set; }
    public ObservableCollection<Fine> Fines { get; private set; }

    public ICommand LogoutCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand PayFineCommand { get; }

    public event EventHandler RequestClose;

    public ProfileViewModel(UserService userService, Action onGoBack, Action onLogout)
    {
        _userService = userService;
        _onGoBack = onGoBack;
        _onLogout = onLogout;

        LogoutCommand = new RelayCommand(Logout);
        GoBackCommand = new RelayCommand(GoBack);
        PayFineCommand = new RelayCommand<Fine>(PayFine);

        LoadUserProfile();
    }

    private void LoadUserProfile()
    {
        UserProfile = _userService.GetCurrentUser();
        CurrentLoans = new ObservableCollection<Loan>(_userService.GetCurrentLoans());
        LoanRequests = new ObservableCollection<LoanRequest>(_userService.GetLoanRequests());
        Fines = new ObservableCollection<Fine>(_userService.GetFines());

        // Notify UI that collections have been updated
        OnPropertyChanged(nameof(CurrentLoans));
        OnPropertyChanged(nameof(LoanRequests));
        OnPropertyChanged(nameof(Fines));
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
}

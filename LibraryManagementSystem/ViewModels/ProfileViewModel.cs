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

/// <summary>
/// ViewModel for managing the user profile view, including viewing and managing current loans, loan requests, and fines.
/// </summary>
public partial class ProfileViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly ItemService _itemService;
    private readonly Action _onGoBack;
    private readonly Action _onLogout;
    private User _userProfile;

    /// <summary>
    /// Gets or sets the current user's profile information.
    /// </summary>
    public User UserProfile
    {
        get => _userProfile;
        private set => SetProperty(ref _userProfile, value);
    }

    /// <summary>
    /// Gets the collection of current loans for the user.
    /// </summary>
    public ObservableCollection<Loan> CurrentLoans { get; }

    /// <summary>
    /// Gets the collection of loan requests made by the user.
    /// </summary>
    public ObservableCollection<LoanRequest> LoanRequests { get; }

    /// <summary>
    /// Gets the collection of fines associated with the user.
    /// </summary>
    public ObservableCollection<Fine> Fines { get; }

    /// <summary>
    /// Command to log out the current user.
    /// </summary>
    public ICommand LogoutCommand { get; }

    /// <summary>
    /// Command to navigate back to the previous view.
    /// </summary>
    public ICommand GoBackCommand { get; }

    /// <summary>
    /// Command to pay a selected fine.
    /// </summary>
    public ICommand PayFineCommand { get; }

    /// <summary>
    /// Command to return a selected loan item.
    /// </summary>
    public ICommand ReturnItemCommand { get; }

    /// <summary>
    /// Command to update the user's profile information.
    /// </summary>
    public ICommand UpdateProfileCommand { get; }

    /// <summary>
    /// Event triggered when a request to close the current view is made.
    /// </summary>
    public event EventHandler RequestClose;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileViewModel"/> class.
    /// </summary>
    /// <param name="userService">Service for managing user authentication and information.</param>
    /// <param name="itemService">Service for managing items and loans.</param>
    /// <param name="onGoBack">Action to execute when the user decides to go back to the previous view.</param>
    /// <param name="onLogout">Action to execute when the user logs out.</param>
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

    /// <summary>
    /// Opens a window to update the user's profile information.
    /// </summary>
    private void UpdateProfile()
    {
        var updateUserWindow = new AddUpdateUserWindow(UserProfile, _userService, isEditable: false);
        updateUserWindow.ShowDialog();
    }

    /// <summary>
    /// Logs out the current user and triggers the logout action.
    /// </summary>
    private void Logout()
    {
        _userService.Logout();
        _onLogout?.Invoke();
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Navigates back to the previous view and triggers the go back action.
    /// </summary>
    private void GoBack()
    {
        _onGoBack?.Invoke();
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Creates a payment request for the selected fine and removes it from the user's list of fines.
    /// </summary>
    /// <param name="fine">The fine to be paid.</param>
    private void PayFine(Fine fine)
    {
        if (fine != null)
        {
            _userService.CreateFinePayRequest(fine);
            Fines.Remove(fine);
        }
    }

    /// <summary>
    /// Returns the selected loan item and removes it from the user's list of current loans.
    /// </summary>
    /// <param name="loan">The loan to be returned.</param>
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

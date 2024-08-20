using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;

/// <summary>
/// ViewModel for managing the login process, handling user authentication and navigation logic.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly Action _onSuccessfulLogin;
    private readonly Action _onGoBack;
    private string _username;
    private string _password;

    /// <summary>
    /// Gets or sets the username entered by the user.
    /// </summary>
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    /// <summary>
    /// Gets or sets the password entered by the user.
    /// </summary>
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    /// <summary>
    /// Command to execute the login process.
    /// </summary>
    public ICommand LoginCommand { get; }

    /// <summary>
    /// Command to navigate back to the previous view.
    /// </summary>
    public ICommand GoBackCommand { get; }

    /// <summary>
    /// Event triggered when a request to close the current view is made.
    /// </summary>
    public event EventHandler RequestClose;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
    /// </summary>
    /// <param name="userService">The service used for managing user authentication and information.</param>
    /// <param name="onSuccessfulLogin">Action to execute after a successful login.</param>
    /// <param name="onGoBack">Action to execute when the user decides to go back to the previous view.</param>
    public LoginViewModel(UserService userService, Action onSuccessfulLogin, Action onGoBack)
    {
        _userService = userService;
        _onSuccessfulLogin = onSuccessfulLogin;
        _onGoBack = onGoBack;

        LoginCommand = new RelayCommand(Login);
        GoBackCommand = new RelayCommand(GoBack);
    }

    /// <summary>
    /// Executes the login process. If successful, it triggers the <see cref="_onSuccessfulLogin"/> action and closes the view.
    /// Otherwise, it displays an error message.
    /// </summary>
    private void Login()
    {
        if (_userService.ValidateUser(Username, Password))
        {
            _userService.Login(Username, Password);
            _onSuccessfulLogin?.Invoke();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Navigates back to the previous view and triggers the <see cref="_onGoBack"/> action.
    /// </summary>
    private void GoBack()
    {
        _onGoBack?.Invoke();
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}

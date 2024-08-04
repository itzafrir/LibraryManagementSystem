using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;

public partial class LoginViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly Action _onSuccessfulLogin;
    private readonly Action _onGoBack;
    private string _username;
    private string _password;

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand GoBackCommand { get; }

    public event EventHandler RequestClose;

    public LoginViewModel(UserService userService, Action onSuccessfulLogin, Action onGoBack)
    {
        _userService = userService;
        _onSuccessfulLogin = onSuccessfulLogin;
        _onGoBack = onGoBack;

        LoginCommand = new RelayCommand(Login);
        GoBackCommand = new RelayCommand(GoBack);
    }

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

    private void GoBack()
    {
        _onGoBack?.Invoke();
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}
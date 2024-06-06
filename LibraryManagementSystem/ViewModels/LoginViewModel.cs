using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Services;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly UserService _userService;
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

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            var user = _userService.ValidateUser(Username, Password);
            if (user != null)
            {
                MessageBox.Show("Login successful!", "Success");
                // Navigate to profile or main window
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error");
            }
        }
    }
}
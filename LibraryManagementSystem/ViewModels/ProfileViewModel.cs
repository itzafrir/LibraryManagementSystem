using CommunityToolkit.Mvvm.ComponentModel;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ProfileViewModel(UserService userService)
        {
            _userService = userService;
            CurrentUser = _userService.GetCurrentUser();
        }
    }
}
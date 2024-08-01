using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Collections.ObjectModel;
using System.Linq;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.ViewModels
{
    public partial class UserManagementViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private string _searchTerm;
        private User _selectedUser;

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<User> FilteredUsers { get; set; }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                SetProperty(ref _searchTerm, value);
                FilterUsers();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public IRelayCommand SearchCommand { get; }
        public IRelayCommand AddUserCommand { get; }
        public IRelayCommand RemoveUserCommand { get; }

        public UserManagementViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>(_userService.GetAllUsers());
            FilteredUsers = new ObservableCollection<User>(Users);
            SearchCommand = new RelayCommand(FilterUsers);
            AddUserCommand = new RelayCommand(AddUser);
            RemoveUserCommand = new RelayCommand(RemoveUser);
        }

        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                FilteredUsers = new ObservableCollection<User>(Users);
            }
            else
            {
                FilteredUsers = new ObservableCollection<User>(
                    Users.Where(u => u.Username.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     u.FullName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     u.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                     u.UserType.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                );
            }

            OnPropertyChanged(nameof(FilteredUsers));
        }

        private void AddUser()
        {
            // Code to open a new window to add user details and save them
            var newUser = new User
            {
                Id = Users.Count + 1, // Simplified unique ID generation for example purposes
                Username = "new_user",
                Password = "password",
                FullName = "New User",
                Email = "new_user@example.com",
                Address = "New Address",
                PhoneNumber = "000-000-0000",
                UserType = UserType.Member,
                MembershipDate = DateTime.Now,
            };

            _userService.AddUser(newUser);
            Users.Add(newUser);
            FilterUsers();
        }

        private void RemoveUser()
        {
            if (SelectedUser != null)
            {
                _userService.RemoveUser(SelectedUser);
                Users.Remove(SelectedUser);
                FilterUsers();
            }
        }
    }
}

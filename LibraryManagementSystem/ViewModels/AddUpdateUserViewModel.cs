using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

public class AddUpdateUserViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly Action _closeWindow;

    public User EditableUser { get; private set; }
    public User OriginalUser { get; private set; }
    public ObservableCollection<UserType> UserTypes { get; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    // New property to control the editability of UserType and MembershipDate
    public bool IsUserTypeAndMembershipDateEditable { get; }

    public AddUpdateUserViewModel(User user, UserService userService, Action closeWindow, bool isEditable = true)
    {
        OriginalUser = user ?? new User();
        EditableUser = new User
        {
            Id = OriginalUser.Id,
            Username = OriginalUser.Username,
            Password = OriginalUser.Password,
            FullName = OriginalUser.FullName,
            Email = OriginalUser.Email,
            Address = OriginalUser.Address,
            PhoneNumber = OriginalUser.PhoneNumber,
            UserType = OriginalUser.UserType,
            MembershipDate = OriginalUser.MembershipDate,
            CurrentLoans = OriginalUser.CurrentLoans,
            LoanRequests = OriginalUser.LoanRequests,
            Fines = OriginalUser.Fines
        };

        _userService = userService;
        _closeWindow = closeWindow;
        IsUserTypeAndMembershipDateEditable = isEditable;  // Set the editability flag

        UserTypes = new ObservableCollection<UserType>((UserType[])Enum.GetValues(typeof(UserType)));

        SaveCommand = new RelayCommand(Save, CanSave);
        CancelCommand = new RelayCommand(Cancel);

        EditableUser.PropertyChanged += (s, e) => ((RelayCommand)SaveCommand).NotifyCanExecuteChanged();
    }

    private void Save()
    {
        // Copy values from EditableUser to OriginalUser
        OriginalUser.Username = EditableUser.Username;
        OriginalUser.Password = EditableUser.Password;
        OriginalUser.FullName = EditableUser.FullName;
        OriginalUser.Email = EditableUser.Email;
        OriginalUser.Address = EditableUser.Address;
        OriginalUser.PhoneNumber = EditableUser.PhoneNumber;

        // Only update these fields if they are editable
        if (IsUserTypeAndMembershipDateEditable)
        {
            OriginalUser.UserType = EditableUser.UserType;
            OriginalUser.MembershipDate = EditableUser.MembershipDate;
        }

        if (OriginalUser.Id == 0)
        {
            _userService.AddUser(OriginalUser);
        }
        else
        {
            _userService.UpdateUser(OriginalUser);
        }

        _closeWindow();
    }

    private void Cancel()
    {
        _closeWindow();
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(EditableUser.Username) &&
               !string.IsNullOrWhiteSpace(EditableUser.FullName) &&
               !string.IsNullOrWhiteSpace(EditableUser.Password) &&
               !string.IsNullOrWhiteSpace(EditableUser.Email) &&
               !string.IsNullOrWhiteSpace(EditableUser.PhoneNumber) &&
               EditableUser.UserType != null;
    }
}

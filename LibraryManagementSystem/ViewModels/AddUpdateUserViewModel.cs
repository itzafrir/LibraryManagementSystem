using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class AddUpdateUserViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly Action _closeWindow;

    /// <summary>
    /// Gets the user being edited.
    /// </summary>
    public User EditableUser { get; private set; }

    /// <summary>
    /// Gets the original user data before any edits.
    /// </summary>
    public User OriginalUser { get; private set; }

    /// <summary>
    /// Gets the collection of user types available for selection.
    /// </summary>
    public ObservableCollection<UserType> UserTypes { get; }

    /// <summary>
    /// Gets the command to save the user.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Gets the command to cancel the operation.
    /// </summary>
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Gets a value indicating whether the UserType and MembershipDate fields are editable.
    /// </summary>
    public bool IsUserTypeAndMembershipDateEditable { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddUpdateUserViewModel"/> class.
    /// </summary>
    /// <param name="user">The user to edit or add. If null, a new user is created.</param>
    /// <param name="userService">The service used for managing users.</param>
    /// <param name="closeWindow">The action to close the window.</param>
    /// <param name="isEditable">Indicates whether the UserType and MembershipDate fields are editable.</param>
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

    /// <summary>
    /// Saves the changes made to the user. If the user is new, they are added; otherwise, the existing user is updated.
    /// </summary>
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

    /// <summary>
    /// Cancels the operation and closes the window without saving changes.
    /// </summary>
    private void Cancel()
    {
        _closeWindow();
    }

    /// <summary>
    /// Determines whether the save command can execute, based on the validity of the user input.
    /// </summary>
    /// <returns><c>true</c> if the save command can execute; otherwise, <c>false</c>.</returns>
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

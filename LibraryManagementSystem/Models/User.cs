using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Represents a user in the library system, including personal details, account status, current loans, loan requests, and fines.
    /// Implements <see cref="INotifyPropertyChanged"/> to support data binding in the UI.
    /// </summary>
    public class User : INotifyPropertyChanged
    {
        private int _id;
        private string _username;
        private string _password;
        private string _fullName;
        private string _email;
        private string _address;
        private string _phoneNumber;
        private UserType _userType;
        private DateTime _membershipDate;
        private List<Loan> _currentLoans;
        private List<LoanRequest> _loanRequests;
        private List<Fine> _fines;

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Gets or sets the username for the user.
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// Gets or sets the physical address of the user.
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        /// <summary>
        /// Gets or sets the type of user (e.g., Admin, Member).
        /// </summary>
        public UserType UserType
        {
            get => _userType;
            set
            {
                _userType = value;
                OnPropertyChanged(nameof(UserType));
            }
        }

        /// <summary>
        /// Gets or sets the date when the user became a member of the library.
        /// </summary>
        public DateTime MembershipDate
        {
            get => _membershipDate;
            set
            {
                _membershipDate = value;
                OnPropertyChanged(nameof(MembershipDate));
            }
        }

        /// <summary>
        /// Gets or sets the list of current loans associated with the user.
        /// </summary>
        public List<Loan> CurrentLoans
        {
            get => _currentLoans;
            set
            {
                _currentLoans = value;
                OnPropertyChanged(nameof(CurrentLoans));
            }
        }

        /// <summary>
        /// Gets or sets the list of loan requests made by the user.
        /// </summary>
        public List<LoanRequest> LoanRequests
        {
            get => _loanRequests;
            set
            {
                _loanRequests = value;
                OnPropertyChanged(nameof(LoanRequests));
            }
        }

        /// <summary>
        /// Gets or sets the list of fines associated with the user.
        /// </summary>
        public List<Fine> Fines
        {
            get => _fines;
            set
            {
                _fines = value;
                OnPropertyChanged(nameof(Fines));
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with default values.
        /// </summary>
        public User()
        {
            MembershipDate = DateTime.Now;
            CurrentLoans = new List<Loan>();
            LoanRequests = new List<LoanRequest>();
            Fines = new List<Fine>();
        }

        /// <summary>
        /// Gets a detailed string representation of the user's profile, including personal details and the total fines owed.
        /// </summary>
        /// <returns>A string containing the user's profile details.</returns>
        public string GetProfileDetails()
        {
            return $"Username: {Username}, Full Name: {FullName}, Email: {Email}, Address: {Address}, Phone Number: {PhoneNumber}, User Type: {UserType}, Membership Date: {MembershipDate.ToShortDateString()}, Fines: {Fines.Sum(f => f.Amount)}";
        }
    }
}

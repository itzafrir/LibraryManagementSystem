using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LibraryManagementSystem.Models
{
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
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        public UserType UserType
        {
            get => _userType;
            set
            {
                _userType = value;
                OnPropertyChanged(nameof(UserType));
            }
        }
        public DateTime MembershipDate
        {
            get => _membershipDate;
            set
            {
                _membershipDate = value;
                OnPropertyChanged(nameof(MembershipDate));
            }
        }
        public List<Loan> CurrentLoans
        {
            get => _currentLoans;
            set
            {
                _currentLoans = value;
                OnPropertyChanged(nameof(CurrentLoans));
            }
        }
        public List<LoanRequest> LoanRequests
        {
            get => _loanRequests;
            set
            {
                _loanRequests = value;
                OnPropertyChanged(nameof(LoanRequests));
            }
        }
        public List<Fine> Fines
        {
            get => _fines;
            set
            {
                _fines = value;
                OnPropertyChanged(nameof(Fines));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User()
        {
            MembershipDate = DateTime.Now;
            CurrentLoans = new List<Loan>();
            LoanRequests = new List<LoanRequest>();
            Fines = new List<Fine>();
        }

        // Method to get user profile details
        public string GetProfileDetails()
        {
            return $"Username: {Username}, Full Name: {FullName}, Email: {Email}, Address: {Address}, Phone Number: {PhoneNumber}, User Type: {UserType}, Membership Date: {MembershipDate.ToShortDateString()}, Fines: {Fines.Sum(f => f.Amount)}";
        }
    }

}

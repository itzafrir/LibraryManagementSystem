using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public DateTime MembershipDate { get; set; }
        public List<Loan> CurrentLoans { get; set; }
        public List<LoanRequest> LoanRequests { get; set; }
        public double Fines { get; set; }

        // Constructor to initialize lists
        public User()
        {
            Id = 0;
            FullName = "OMER";
            Username = "OMER";
            Password = "OMER";
            Email = "OMER";
            Address = "OMER";
            PhoneNumber = "OMER";
            UserType = UserType.Assistant;
            MembershipDate = DateTime.Now;
            Fines = 0;
            CurrentLoans = new List<Loan>();
            LoanRequests = new List<LoanRequest>();
        }

        // Method to request a loan
        public void RequestLoan(Item item)
        {
            LoanRequest loanRequest = new LoanRequest
            {
                ItemId = item.Id,
                UserId = this.Id,
                RequestDate = DateTime.Now
            };

            LoanRequests.Add(loanRequest);
        }

        // Method to add a loan
        public void AddLoan(Loan loan)
        {
            CurrentLoans.Add(loan);
        }

        // Method to return a loan
        public void ReturnLoan(Loan loan)
        {
            loan.ReturnLoan();
            CurrentLoans.Remove(loan);
        }

        // Method to pay fines
        public void PayFine(double amount)
        {
            Fines -= amount;
        }

        // Method to get user profile details
        public string GetProfileDetails()
        {
            return $"Username: {Username}, Full Name: {FullName}, Email: {Email}, Address: {Address}, Phone Number: {PhoneNumber}, User Type: {UserType}, Membership Date: {MembershipDate.ToShortDateString()}, Fines: {Fines}";
        }
    }
}

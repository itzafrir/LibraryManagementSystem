using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class UserService
    {
        private readonly List<User> _users;
        private readonly List<FinePayRequest> _finePayRequests;
        private readonly List<LoanRequest> _loanRequests;
        private User _currentUser;

        public UserService()
        {
            // Dummy data for demonstration purposes
            _users = new List<User>
            {
                new User { Id = 1, Username = "john_doe", Password = "d", FullName = "John Doe", Email = "john@example.com", UserType = UserType.Member, MembershipDate = DateTime.Now.AddYears(-2) },
                new User { Id = 2, Username = "jane_smith", Password = "d", FullName = "Jane Smith", Email = "jane@example.com", UserType = UserType.Librarian, MembershipDate = DateTime.Now.AddYears(-3) },
                new User { Id = 3, Username = "a", Password = "a", FullName = "Sam Green", Email = "sam@example.com", UserType = UserType.Guest, MembershipDate = DateTime.Now.AddMonths(-6) },
                // Add more users as needed
            };

            // Add fines to users
            _users[0].Fines.Add(new Fine { Id = 1, UserId = 1, Amount = 5.0, DateIssued = DateTime.Now.AddDays(-30), DatePaid = null });
            _users[1].Fines.Add(new Fine { Id = 2, UserId = 2, Amount = 5.0, DateIssued = DateTime.Now.AddDays(-15), DatePaid = null });
            _users[2].Fines.Add(new Fine { Id = 3, UserId = 3, Amount = 10.0, DateIssued = DateTime.Now.AddDays(-10), DatePaid = null });

            _finePayRequests = new List<FinePayRequest>();
            _loanRequests = new List<LoanRequest>();
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public void RemoveUser(User user)
        {
            var userToRemove = _users.FirstOrDefault(u => u.Id == user.Id);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
            }
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public List<Loan> GetCurrentLoans()
        {
            // Retrieve loans for the current user
            return new List<Loan>
            {
                new Loan { Id = 1, ItemId = 1, UserId = _currentUser.Id, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(4), LoanStatus = LoanStatus.Active, Item = new Book { Title = "The Great Gatsby" } }
            };
        }

        public List<LoanRequest> GetLoanRequests()
        {
            return _loanRequests.Where(r => r.UserId == _currentUser.Id).ToList();
        }

        public List<Fine> GetFines()
        {
            // Retrieve fines for the current user
            return _currentUser?.Fines ?? new List<Fine>();
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public User? ValidateUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null && user.Password == password)
            {
                _currentUser = user;
                return user;
            }
            return null;
        }

        public bool IsUserLoggedIn()
        {
            return _currentUser != null;
        }

        public void CreateFinePayRequest(Fine fine)
        {
            var request = new FinePayRequest
            {
                FineId = fine.Id,
                UserId = _currentUser.Id,
                RequestDate = DateTime.Now,
                Status = FinePayRequestStatus.Pending
            };

            _finePayRequests.Add(request);
        }

        public List<FinePayRequest> GetFinePayRequests()
        {
            return _finePayRequests;
        }

        public void ApproveFinePayRequest(FinePayRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Id == request.UserId);
            var fine = user?.Fines.FirstOrDefault(f => f.Id == request.FineId);
            if (fine != null)
            {
                fine.DatePaid = DateTime.Now;
                request.Status = FinePayRequestStatus.Approved;
            }
        }

        public void RejectFinePayRequest(FinePayRequest request)
        {
            request.Status = FinePayRequestStatus.Rejected;
        }
    }
}

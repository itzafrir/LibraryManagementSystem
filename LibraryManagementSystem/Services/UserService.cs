using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly List<FinePayRequest> _finePayRequests;
        private readonly List<LoanRequest> _loanRequests;
        private User _currentUser;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _finePayRequests = new List<FinePayRequest>();
            _loanRequests = new List<LoanRequest>();
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public void RemoveUser(User user)
        {
            var userToRemove = _userRepository.GetById(user.Id);
            if (userToRemove != null)
            {
                _userRepository.Delete(user.Id);
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
            var user = _userRepository.GetById(_currentUser.Id);
            return user?.Fines ?? new List<Fine>();
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public User? ValidateUser(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);
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

        public User GetUserById(int userId)
        {
            return _userRepository.GetById(userId);
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
            var user = _userRepository.GetById(request.UserId);
            var fine = user?.Fines.FirstOrDefault(f => f.Id == request.FineId);
            if (fine != null)
            {
                fine.DatePaid = DateTime.Now;
                request.Status = FinePayRequestStatus.Approved;
                _userRepository.Update(user);
            }
        }

        public void RejectFinePayRequest(FinePayRequest request)
        {
            request.Status = FinePayRequestStatus.Rejected;
        }
    }
}

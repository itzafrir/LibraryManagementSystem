using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Windows;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<LoanRequest> _loanRequestRepository;
        private readonly IRepository<Fine> _fineRepository;
        private readonly IRepository<FinePayRequest> _finePayRequestRepository;

        private User _currentUser;

        public UserService(
            IRepository<User> userRepository,
            IRepository<Loan> loanRepository,
            IRepository<LoanRequest> loanRequestRepository,
            IRepository<Fine> fineRepository,
            IRepository<FinePayRequest> finePayRequestRepository)
        {
            _userRepository = userRepository;
            _loanRepository = loanRepository;
            _loanRequestRepository = loanRequestRepository;
            _fineRepository = fineRepository;
            _finePayRequestRepository = finePayRequestRepository;
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public void Login(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                _currentUser = user;
            }
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public bool IsUserLoggedIn()
        {
            return _currentUser != null;
        }

        public IEnumerable<Loan> GetCurrentLoans()
        {
            return _currentUser != null
                ? _loanRepository.GetAll().Where(l => l.UserId == _currentUser.Id && l.LoanStatus == LoanStatus.Active).ToList()
                : new List<Loan>();
        }

        public IEnumerable<LoanRequest> GetLoanRequests()
        {
            return _currentUser != null
                ? _loanRequestRepository.GetAll().Where(lr => lr.UserId == _currentUser.Id).ToList()
                : new List<LoanRequest>();
        }

        public IEnumerable<Fine> GetFines()
        {
            return _currentUser != null
                ? _fineRepository.GetAll().Where(f => f.UserId == _currentUser.Id && f.Status != FineStatus.Paid).ToList()
                : new List<Fine>();
        }

        public IEnumerable<FinePayRequest> GetFinePayRequests()
        {
            return _finePayRequestRepository.GetAll().ToList();
        }

        public void ApproveFinePayRequest(FinePayRequest finePayRequest)
        {
            var fine = _fineRepository.GetById(finePayRequest.FineId);
            if (fine != null)
            {
                fine.DatePaid = DateTime.Now;
                fine.Status = FineStatus.Paid;
                _fineRepository.Update(fine);
                _finePayRequestRepository.Delete(finePayRequest.Id);
            }
        }

        public void RejectFinePayRequest(FinePayRequest finePayRequest)
        {
            var fine = _fineRepository.GetById(finePayRequest.FineId);
            if (fine != null)
            {
                fine.DatePaid = DateTime.Now;
                fine.Status = FineStatus.Unpaid;
                _fineRepository.Update(fine);
                _finePayRequestRepository.Delete(finePayRequest.Id);
            }
        }

        public void CreateFinePayRequest(Fine fine)
        {
            var finePayRequest = new FinePayRequest
            {
                FineId = fine.Id,
                UserId = fine.UserId,
                RequestDate = DateTime.Now
            };

            _finePayRequestRepository.Add(finePayRequest);
            fine.Status = FineStatus.Pending;
            _fineRepository.Update(fine);
        }

        public void CalculateFines()
        {
            var overdueLoans = _loanRepository.GetAll().Where(l => l.UserId == _currentUser.Id && l.IsOverdue());
            foreach (var loan in overdueLoans)
            {
                var existingFine = _fineRepository.GetAll().FirstOrDefault(f => f.UserId == loan.UserId && f.ItemId == loan.ItemId && f.Status == FineStatus.Unpaid);
                if (existingFine == null)
                {
                    var monthsOverdue = (DateTime.Now - loan.DueDate).Days / 30;
                    var fineAmount = monthsOverdue * 1; // $1 per month overdue

                    var fine = new Fine
                    {
                        UserId = loan.UserId,
                        ItemId = loan.ItemId,
                        Amount = fineAmount,
                        DateIssued = DateTime.Now,
                        Status = FineStatus.Unpaid
                    };
                    _fineRepository.Add(fine);
                }
            }
        }

        public void RequestLoan(Item item)
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("User must be logged in to request a loan.");
            }

            var loanRequest = new LoanRequest
            {
                ItemId = item.Id,
                UserId = _currentUser.Id,
                RequestDate = DateTime.Now
            };

            _loanRequestRepository.Add(loanRequest);
        }

        public void RemoveLoanRequest(LoanRequest loanRequest)
        {
            var user = loanRequest.User;
            user.LoanRequests.Remove(loanRequest);
            _userRepository.Update(user);
        }

        public void AddLoan(Loan loan)
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("User must be logged in to add a loan.");
            }

            loan.UserId = _currentUser.Id;
            _loanRepository.Add(loan);
        }

        public void ReturnLoan(Loan loan)
        {
            if (loan.LoanStatus != LoanStatus.Active)
            {
                throw new InvalidOperationException("Only active loans can be returned.");
            }

            loan.ReturnLoan();
            _loanRepository.Update(loan);
        }

        public void PayFine(double amount)
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("User must be logged in to pay a fine.");
            }

            var unpaidFine = _fineRepository.GetAll().FirstOrDefault(f => f.UserId == _currentUser.Id && f.Status == FineStatus.Unpaid);
            if (unpaidFine != null)
            {
                unpaidFine.DatePaid = DateTime.Now;
                unpaidFine.Amount -= amount;
                if (unpaidFine.Amount <= 0)
                {
                    unpaidFine.Status = FineStatus.Paid;
                }
                _fineRepository.Update(unpaidFine);
            }
        }

        public void RemoveLoanFromUser(Loan loan)
        {
            var user = _userRepository.GetById(loan.UserId);
            user.CurrentLoans.Remove(loan);
            _userRepository.Update(user);
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public bool DeleteUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                // Check for active loans
                var activeLoans = _loanRepository.GetAll().Where(l => l.UserId == userId && l.LoanStatus == LoanStatus.Active).ToList();

                // Check for pending loan requests
                var pendingLoanRequests = _loanRequestRepository.GetAll().Where(lr => lr.UserId == userId).ToList();

                // Check for outstanding fines
                var outstandingFines = _fineRepository.GetAll().Where(f => f.UserId == userId && f.Status != FineStatus.Paid).ToList();

                // Check for pending fine payment requests
                var pendingFinePayRequests = _finePayRequestRepository.GetAll().Where(fpr => fpr.UserId == userId).ToList();

                if (activeLoans.Any() || pendingLoanRequests.Any() || outstandingFines.Any() || pendingFinePayRequests.Any())
                {
                    // Prepare a generic message with the reasons why the user cannot be deleted
                    string message = "This user cannot be deleted because they are currently associated with:\n\n";

                    if (activeLoans.Any())
                    {
                        message += $"- {activeLoans.Count} active loan(s)\n";
                    }

                    if (pendingLoanRequests.Any())
                    {
                        message += $"- {pendingLoanRequests.Count} pending loan request(s)\n";
                    }

                    if (outstandingFines.Any())
                    {
                        message += $"- {outstandingFines.Count} outstanding fine(s)\n";
                    }

                    if (pendingFinePayRequests.Any())
                    {
                        message += $"- {pendingFinePayRequests.Count} pending fine payment request(s)\n";
                    }

                    // Display the message to the user
                    MessageBox.Show(message, "User Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Warning);

                    // Return false to indicate the deletion was not successful
                    return false;
                }

                // If no blocking associations exist, proceed with deletion
                _userRepository.Delete(userId);
                return true; // Return true to indicate the deletion was successful
            }

            return false; // User not found, deletion not successful
        }



        public bool ValidateUser(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public List<LoanRequest> GetLoanRequestsForItem(int itemId)
        {
            return _loanRequestRepository.GetAll().Where(lr => lr.ItemId == itemId).ToList();
        }

        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            return _userRepository.GetAll().Where(u => u.Username.Contains(searchTerm) || u.FullName.Contains(searchTerm));
        }
    }
}

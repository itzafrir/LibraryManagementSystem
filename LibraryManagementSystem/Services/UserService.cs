using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
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
            return _currentUser != null
                ? _finePayRequestRepository.GetAll().Where(fpr => fpr.UserId == _currentUser.Id).ToList()
                : new List<FinePayRequest>();
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
            _finePayRequestRepository.Delete(finePayRequest.Id);
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

        public User GetUserById(int userId)
        {
            return _userRepository.GetById(userId);
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
    }
}

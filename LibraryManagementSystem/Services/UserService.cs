using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Utilities;

namespace LibraryManagementSystem.Services
{
    /// <summary>
    /// Provides services for managing users in the library system, including login, loans, fines, and user CRUD operations.
    /// </summary>
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<LoanRequest> _loanRequestRepository;
        private readonly IRepository<Fine> _fineRepository;
        private readonly IRepository<FinePayRequest> _finePayRequestRepository;

        private User _currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class with the specified repositories.
        /// </summary>
        /// <param name="userRepository">The repository for managing users.</param>
        /// <param name="loanRepository">The repository for managing loans.</param>
        /// <param name="loanRequestRepository">The repository for managing loan requests.</param>
        /// <param name="fineRepository">The repository for managing fines.</param>
        /// <param name="finePayRequestRepository">The repository for managing fine payment requests.</param>
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

        /// <summary>
        /// Gets the currently logged-in user.
        /// </summary>
        /// <returns>The current <see cref="User"/>.</returns>
        public User GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Logs in a user with the specified username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        public void Login(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                _currentUser = user;
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Checks if a user is currently logged in.
        /// </summary>
        /// <returns><c>true</c> if a user is logged in; otherwise, <c>false</c>.</returns>
        public bool IsUserLoggedIn()
        {
            return _currentUser != null;
        }

        /// <summary>
        /// Gets the current active loans of the logged-in user.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Loan}"/> containing the current loans.</returns>
        public IEnumerable<Loan> GetCurrentLoans()
        {
            return _currentUser != null
                ? _loanRepository.GetAll().Where(l => l.UserId == _currentUser.Id && l.LoanStatus == LoanStatus.Active).ToList()
                : new List<Loan>();
        }

        /// <summary>
        /// Gets the loan requests of the logged-in user.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{LoanRequest}"/> containing the loan requests.</returns>
        public IEnumerable<LoanRequest> GetLoanRequests()
        {
            return _currentUser != null
                ? _loanRequestRepository.GetAll().Where(lr => lr.UserId == _currentUser.Id).ToList()
                : new List<LoanRequest>();
        }

        /// <summary>
        /// Gets the unpaid fines of the logged-in user.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Fine}"/> containing the unpaid fines.</returns>
        public IEnumerable<Fine> GetFines()
        {
            return _currentUser != null
                ? _fineRepository.GetAll().Where(f => f.UserId == _currentUser.Id && f.Status != FineStatus.Paid).ToList()
                : new List<Fine>();
        }

        /// <summary>
        /// Gets all fine payment requests.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{FinePayRequest}"/> containing the fine payment requests.</returns>
        public IEnumerable<FinePayRequest> GetFinePayRequests()
        {
            return _finePayRequestRepository.GetAll().ToList();
        }

        /// <summary>
        /// Approves a fine payment request.
        /// </summary>
        /// <param name="finePayRequest">The fine payment request to approve.</param>
        /// <returns>A message indicating the result of the approval.</returns>
        public string ApproveFinePayRequest(FinePayRequest finePayRequest)
        {
            var fine = _fineRepository.GetById(finePayRequest.FineId);
            if (fine != null)
            {
                var activeLoan = _loanRepository.GetAll()
                    .FirstOrDefault(l => l.ItemId == fine.ItemId && l.UserId == fine.UserId && l.LoanStatus == LoanStatus.Active);

                if (activeLoan != null)
                {
                    return "Cannot approve fine payment while there is an active loan on the item.";
                }

                fine.DatePaid = DateTime.Now;
                fine.Status = FineStatus.Paid;
                _fineRepository.Update(fine);
                _finePayRequestRepository.Delete(finePayRequest.Id);

                return "Success";
            }

            return "Fine not found.";
        }

        /// <summary>
        /// Rejects a fine payment request.
        /// </summary>
        /// <param name="finePayRequest">The fine payment request to reject.</param>
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

        /// <summary>
        /// Creates a fine payment request for a specified fine.
        /// </summary>
        /// <param name="fine">The fine for which to create a payment request.</param>
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

        /// <summary>
        /// Requests a loan for a specified item.
        /// </summary>
        /// <param name="item">The item to request a loan for.</param>
        /// <exception cref="InvalidOperationException">Thrown if the user is not logged in.</exception>
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

        /// <summary>
        /// Removes a loan request from a user.
        /// </summary>
        /// <param name="loanRequest">The loan request to remove.</param>
        public void RemoveLoanRequest(LoanRequest loanRequest)
        {
            var user = loanRequest.User;
            user.LoanRequests.Remove(loanRequest);
            _userRepository.Update(user);
        }

        /// <summary>
        /// Adds a loan for the logged-in user.
        /// </summary>
        /// <param name="loan">The loan to add.</param>
        /// <exception cref="InvalidOperationException">Thrown if the user is not logged in.</exception>
        public void AddLoan(Loan loan)
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("User must be logged in to add a loan.");
            }

            loan.UserId = _currentUser.Id;
            _loanRepository.Add(loan);
        }

        /// <summary>
        /// Returns a loan.
        /// </summary>
        /// <param name="loan">The loan to return.</param>
        /// <exception cref="InvalidOperationException">Thrown if the loan is not active.</exception>
        public void ReturnLoan(Loan loan)
        {
            if (loan.LoanStatus != LoanStatus.Active)
            {
                throw new InvalidOperationException("Only active loans can be returned.");
            }

            loan.ReturnLoan();
            _loanRepository.Update(loan);
        }

        /// <summary>
        /// Pays a fine for the logged-in user.
        /// </summary>
        /// <param name="amount">The amount to pay.</param>
        /// <exception cref="InvalidOperationException">Thrown if the user is not logged in.</exception>
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

        /// <summary>
        /// Removes a loan from the user.
        /// </summary>
        /// <param name="loan">The loan to remove.</param>
        public void RemoveLoanFromUser(Loan loan)
        {
            var user = _userRepository.GetById(loan.UserId);
            user.CurrentLoans.Remove(loan);
            _userRepository.Update(user);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The <see cref="User"/> with the specified ID.</returns>
        public User GetUserById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{User}"/> containing all users.</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="user">The user to add.</param>
        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        /// <summary>
        /// Updates an existing user in the system.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        /// <summary>
        /// Deletes a user from the system if there are no active loans, pending loan requests, outstanding fines, or pending fine payment requests.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns><c>true</c> if the user was successfully deleted; otherwise, <c>false</c>.</returns>
        public bool DeleteUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                var activeLoans = _loanRepository.GetAll().Where(l => l.UserId == userId && l.LoanStatus == LoanStatus.Active).ToList();
                var pendingLoanRequests = _loanRequestRepository.GetAll().Where(lr => lr.UserId == userId).ToList();
                var outstandingFines = _fineRepository.GetAll().Where(f => f.UserId == userId && f.Status != FineStatus.Paid).ToList();
                var pendingFinePayRequests = _finePayRequestRepository.GetAll().Where(fpr => fpr.UserId == userId).ToList();

                if (activeLoans.Any() || pendingLoanRequests.Any() || outstandingFines.Any() || pendingFinePayRequests.Any())
                {
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

                    MessageBox.Show(message, "User Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                _userRepository.Delete(userId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates a user's credentials.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        /// <returns><c>true</c> if the credentials are valid; otherwise, <c>false</c>.</returns>
        public bool ValidateUser(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            return user != null;
        }

        /// <summary>
        /// Gets the loan requests associated with a specific item.
        /// </summary>
        /// <param name="itemId">The ID of the item.</param>
        /// <returns>A list of <see cref="LoanRequest"/> objects associated with the item.</returns>
        public List<LoanRequest> GetLoanRequestsForItem(int itemId)
        {
            return _loanRequestRepository.GetAll().Where(lr => lr.ItemId == itemId).ToList();
        }

        /// <summary>
        /// Searches for users based on a search term that matches the username or full name.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>An <see cref="IEnumerable{User}"/> containing the matching users.</returns>
        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            return _userRepository.GetAll().Where(u => u.Username.Contains(searchTerm) || u.FullName.Contains(searchTerm));
        }
    }
}

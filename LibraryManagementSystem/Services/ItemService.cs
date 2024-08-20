using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    /// <summary>
    /// Provides services for managing items in the library system, including loans, reservations, reviews, and item CRUD operations.
    /// </summary>
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<LoanRequest> _loanRequestRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemService"/> class with the specified repositories and services.
        /// </summary>
        /// <param name="itemRepository">The repository for managing items.</param>
        /// <param name="loanRepository">The repository for managing loans.</param>
        /// <param name="loanRequestRepository">The repository for managing loan requests.</param>
        /// <param name="reviewRepository">The repository for managing reviews.</param>
        /// <param name="userService">The user service for managing user-related operations.</param>
        public ItemService(
            IRepository<Item> itemRepository,
            IRepository<Loan> loanRepository,
            IRepository<LoanRequest> loanRequestRepository,
            IRepository<Review> reviewRepository,
            UserService userService)
        {
            _itemRepository = itemRepository;
            _loanRepository = loanRepository;
            _loanRequestRepository = loanRequestRepository;
            _reviewRepository = reviewRepository;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all items in the library.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Item}"/> containing all items.</returns>
        public IEnumerable<Item> GetAllItems()
        {
            return _itemRepository.GetAll();
        }

        /// <summary>
        /// Retrieves items of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of items to retrieve.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> containing items of the specified type.</returns>
        public IEnumerable<T> GetItemsByType<T>() where T : Item
        {
            return _itemRepository.GetAll().OfType<T>();
        }

        /// <summary>
        /// Searches for items based on a search term that matches the title, ISBN, or creator.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>An <see cref="IEnumerable{Item}"/> containing the matching items.</returns>
        public IEnumerable<Item> SearchItems(string searchTerm)
        {
            return _itemRepository.GetAll()
                .Where(i => i.Title.Contains(searchTerm) || i.ISBN.Contains(searchTerm) || i.GetCreator().Contains(searchTerm));
        }

        /// <summary>
        /// Loans an item to a user if available, or places a reservation if not.
        /// </summary>
        /// <param name="user">The user who wants to loan the item.</param>
        /// <param name="item">The item to be loaned or reserved.</param>
        /// <exception cref="ArgumentNullException">Thrown if the user is null.</exception>
        public void LoanItem(User user, Item item)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (item.AvailableCopies > 0 && !user.CurrentLoans.Any(l => l.ItemId == item.Id))
            {
                item.AvailableCopies--;
                var loan = new Loan
                {
                    ItemId = item.Id,
                    UserId = user.Id,
                    LoanDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(Constants.LOAN_LENGTH),
                    LoanStatus = LoanStatus.Active
                };
                _loanRepository.Add(loan);
                _itemRepository.Update(item);
                MessageBox.Show("Item Loaned", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ReserveItem(user, item);
                MessageBox.Show("No available copies. Loan request has been placed successfully.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Reserves an item for a user by placing a loan request.
        /// </summary>
        /// <param name="user">The user requesting the reservation.</param>
        /// <param name="item">The item to be reserved.</param>
        public void ReserveItem(User user, Item item)
        {
            if (!user.LoanRequests.Any(l => l.ItemId == item.Id))
            {
                var loanRequest = new LoanRequest
                {
                    ItemId = item.Id,
                    UserId = user.Id,
                    RequestDate = DateTime.Now
                };
                _loanRequestRepository.Add(loanRequest);
                user.LoanRequests.Add(loanRequest);
            }
            else
            {
                MessageBox.Show("User already has a loan request for this item");
            }
        }

        /// <summary>
        /// Returns a loaned item, updating loan status and processing any pending loan requests.
        /// </summary>
        /// <param name="loan">The loan to be returned.</param>
        public void ReturnLoan(Loan loan)
        {
            loan.ReturnLoan();
            _loanRepository.Update(loan);

            var item = _itemRepository.GetById(loan.ItemId);
            item.AvailableCopies++;
            _itemRepository.Update(item);

            _userService.RemoveLoanFromUser(loan);
            ProcessLoanRequests(item);
        }

        /// <summary>
        /// Adds a new item to the library.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        public void AddItem(Item item)
        {
            item.GenerateISBN();
            _itemRepository.Add(item);
        }

        /// <summary>
        /// Updates an existing item in the library and processes any pending loan requests.
        /// </summary>
        /// <param name="item">The item to be updated.</param>
        public void UpdateItem(Item item)
        {
            _itemRepository.Update(item);
            ProcessLoanRequests(item);
        }

        /// <summary>
        /// Deletes an item from the library if no active loans or loan requests exist.
        /// </summary>
        /// <param name="itemId">The ID of the item to be deleted.</param>
        /// <returns><c>true</c> if the item was successfully deleted; otherwise, <c>false</c>.</returns>
        public bool DeleteItem(int itemId)
        {
            var item = _itemRepository.GetById(itemId);
            if (item != null)
            {
                var activeLoans = _loanRepository.GetAll().Where(l => l.ItemId == itemId && l.LoanStatus == LoanStatus.Active).ToList();
                var loanRequests = _loanRequestRepository.GetAll().Where(lr => lr.ItemId == itemId).ToList();

                if (activeLoans.Any() || loanRequests.Any())
                {
                    string message = "This item cannot be deleted because it is currently associated with:\n\n";

                    if (activeLoans.Any())
                    {
                        message += "Active Loans:\n";
                        foreach (var loan in activeLoans)
                        {
                            var user = _userService.GetUserById(loan.UserId);
                            message += $"- User: {user.FullName}, Loan Date: {loan.LoanDate.ToShortDateString()}\n";
                        }
                    }

                    if (loanRequests.Any())
                    {
                        message += "\nLoan Requests:\n";
                        foreach (var request in loanRequests)
                        {
                            var user = _userService.GetUserById(request.UserId);
                            message += $"- User: {user.FullName}, Request Date: {request.RequestDate.ToShortDateString()}\n";
                        }
                    }

                    MessageBox.Show(message, "Item Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                var reviewsToDelete = _reviewRepository.GetAll().Where(r => r.ItemId == itemId).ToList();
                foreach (var review in reviewsToDelete)
                {
                    _reviewRepository.Delete(review.Id);
                }

                _itemRepository.Delete(item.Id);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item to retrieve.</param>
        /// <returns>The <see cref="Item"/> with the specified ID.</returns>
        public Item GetItemById(int id)
        {
            return _itemRepository.GetById(id);
        }

        /// <summary>
        /// Adds a review to an item and updates the item.
        /// </summary>
        /// <param name="item">The item being reviewed.</param>
        /// <param name="review">The review to be added.</param>
        public void AddReview(Item item, Review review)
        {
            _reviewRepository.Add(review);
            _itemRepository.Update(item);
        }

        /// <summary>
        /// Validates the properties of an item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>A string containing any validation errors, or an empty string if no errors are found.</returns>
        public string ValidateItemProperties(Item item)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(item.Title))
            {
                errors.Add("Title cannot be empty.");
            }

            if (!DateTime.TryParse(item.PublicationDate.ToString(), out _))
            {
                errors.Add("Publication Date must be a valid date.");
            }

            if (item.TotalCopies < 1)
            {
                errors.Add("Total Copies must be at least 1.");
            }

            if (item.AvailableCopies > item.TotalCopies)
            {
                errors.Add("Available Copies cannot exceed Total Copies.");
            }

            return string.Join(Environment.NewLine, errors);
        }

        /// <summary>
        /// Processes pending loan requests for an item.
        /// </summary>
        /// <param name="item">The item for which loan requests should be processed.</param>
        private void ProcessLoanRequests(Item item)
        {
            var loanRequests = _loanRequestRepository.GetAll()
                .Where(lr => lr.ItemId == item.Id)
                .OrderBy(lr => lr.RequestDate)
                .ToList();

            while (item.AvailableCopies > 0 && loanRequests.Count > 0)
            {
                var loanRequest = loanRequests.First();
                var user = _userService.GetUserById(loanRequest.UserId);

                LoanItem(user, item);

                _loanRequestRepository.Delete(loanRequest.Id);
                loanRequests.RemoveAt(0);
                _userService.RemoveLoanRequest(loanRequest);
            }
        }
    }
}

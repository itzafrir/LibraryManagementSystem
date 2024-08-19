using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LibraryManagementSystem.Utilities.Enums;
using LibraryManagementSystem.Utilities;

namespace LibraryManagementSystem.Services
{
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<LoanRequest> _loanRequestRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly UserService _userService;

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

        public IEnumerable<Item> GetAllItems()
        {
            return _itemRepository.GetAll();
        }

        public IEnumerable<T> GetItemsByType<T>() where T : Item
        {
            return _itemRepository.GetAll().OfType<T>();
        }

        public IEnumerable<Item> SearchItems(string searchTerm)
        {
            return _itemRepository.GetAll()
                .Where(i => i.Title.Contains(searchTerm) || i.ISBN.Contains(searchTerm) || i.GetCreator().Contains(searchTerm));
        }

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
        public void ReturnLoan(Loan loan)
        {
            // Step 1: Update loan status to Returned
            loan.ReturnLoan();
            _loanRepository.Update(loan);

            // Step 2: Increment the available copies of the item
            var item = _itemRepository.GetById(loan.ItemId);
            item.AvailableCopies++;
            _itemRepository.Update(item);

            // Step 3: Update the user's current loans by removing the returned loan
            _userService.RemoveLoanFromUser(loan);

            // Step 4: Process any pending loan requests if copies are now available
            ProcessLoanRequests(item);
        }

        public void AddItem(Item item)
        {
            item.GenerateISBN();
            _itemRepository.Add(item);
        }
        public void UpdateItem(Item item)
        {
            _itemRepository.Update(item);
            ProcessLoanRequests(item);
        }

        public bool DeleteItem(int itemId)
        {
            var item = _itemRepository.GetById(itemId);
            if (item != null)
            {
                // Check for active loans
                var activeLoans = _loanRepository.GetAll().Where(l => l.ItemId == itemId && l.LoanStatus == LoanStatus.Active).ToList();

                // Check for loan requests
                var loanRequests = _loanRequestRepository.GetAll().Where(lr => lr.ItemId == itemId).ToList();

                if (activeLoans.Any() || loanRequests.Any())
                {
                    // Prepare the message with details about relevant users
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

                    // Display the message to the user
                    MessageBox.Show(message, "Item Deletion Canceled", MessageBoxButton.OK, MessageBoxImage.Warning);

                    // Return false to indicate the deletion was not successful
                    return false;
                }
                // Delete all associated reviews
                var reviewsToDelete = _reviewRepository.GetAll().Where(r => r.ItemId == itemId).ToList();
                foreach (var review in reviewsToDelete)
                {
                    _reviewRepository.Delete(review.Id);
                }

                // If no active loans or loan requests exist, proceed with deletion
                _itemRepository.Delete(item.Id);
                return true; // Return true to indicate the deletion was successful
            }

            return false; // Item not found, deletion not successful
        }

        public Item GetItemById(int id)
        {
            return _itemRepository.GetById(id);
        }
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

                // Create a new loan for the user
                LoanItem(user, item);

                // Remove the processed loan request
                _loanRequestRepository.Delete(loanRequest.Id);
                loanRequests.RemoveAt(0);
                //Remove the current loan from the user
                _userService.RemoveLoanRequest(loanRequest);
            }
        }

        public void AddReview(Item item, Review review)
        {
            _reviewRepository.Add(review);
            _itemRepository.Update(item);
        }
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
    }
}

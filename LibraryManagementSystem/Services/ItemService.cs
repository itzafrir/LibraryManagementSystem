using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Loan> _loanRepository;
        private readonly IRepository<LoanRequest> _loanRequestRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly UserService _userService;

        public ItemService(IRepository<Item> itemRepository, IRepository<Loan> loanRepository, IRepository<LoanRequest> loanRequestRepository, IRepository<Review> reviewRepository, UserService userService)
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

        public IEnumerable<Item> GetItemsByType(ItemType itemType)
        {
            return _itemRepository.GetAll().Where(i => i.ItemType == itemType);
        }

        public IEnumerable<Item> SearchItems(string searchTerm)
        {
            return _itemRepository.GetAll()
                .Where(i => i.Title.Contains(searchTerm) || i.ISBN.Contains(searchTerm) || i.GetCreator().Contains(searchTerm));
        }

        public void LoanItem(User user, Item item)
        {
            if (item.AvailableCopies > 0)
            {
                item.AvailableCopies--;
                var loan = new Loan
                {
                    ItemId = item.Id,
                    UserId = user.Id,
                    LoanDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    LoanStatus = LoanStatus.Active
                };
                _loanRepository.Add(loan);
                _itemRepository.Update(item);
            }
            else
            {
                ReserveItem(user, item);
            }
        }

        public void ReturnItem(Loan loan)
        {
            var item = _itemRepository.GetById(loan.ItemId);
            loan.ReturnLoan();
            _loanRepository.Update(loan);

            item.AvailableCopies++;
            _itemRepository.Update(item);

            ProcessLoanRequests(item);
        }

        public void ReserveItem(User user, Item item)
        {
            var loanRequest = new LoanRequest
            {
                ItemId = item.Id,
                UserId = user.Id,
                RequestDate = DateTime.Now
            };
            _loanRequestRepository.Add(loanRequest);
        }

        private void ProcessLoanRequests(Item item)
        {
            var loanRequests = _loanRequestRepository.GetAll().Where(lr => lr.ItemId == item.Id).OrderBy(lr => lr.RequestDate).ToList();

            while (item.AvailableCopies > 0 && loanRequests.Count > 0)
            {
                var loanRequest = loanRequests.First();
                var user = _userService.GetUserById(loanRequest.UserId);
                LoanItem(user, item);
                _loanRequestRepository.Delete(loanRequest.Id);
                loanRequests.RemoveAt(0);
            }
        }

        public void AddReview(Item item, Review review)
        {
            // Check if the review already exists
            if (!_reviewRepository.GetAll().Any(r => r.ItemId == review.ItemId && r.UserId == review.UserId))
            {
                _reviewRepository.Add(review);
                _itemRepository.Update(item); // Update the item in the database
            }
            else
            {
                throw new InvalidOperationException("This review already exists.");
            }
        }

        public void UpdateItem(Item item)
        {
            _itemRepository.Update(item);
        }
    }
}

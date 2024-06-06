using System;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class ItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Loan> _loanRepository;

        public ItemService(IRepository<Item> itemRepository, IRepository<Loan> loanRepository)
        {
            _itemRepository = itemRepository;
            _loanRepository = loanRepository;
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
            var loan = new Loan
            {
                ItemId = item.Id,
                UserId = user.Id,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                LoanStatus = LoanStatus.Active
            };
            _loanRepository.Add(loan);
        }

        public void ReturnItem(Loan loan)
        {
            loan.ReturnLoan();
            _loanRepository.Update(loan);
        }

        public void ReserveItem(User user, Item item)
        {
            var loanRequest = new LoanRequest
            {
                ItemId = item.Id,
                UserId = user.Id,
                RequestDate = DateTime.Now
            };
            // Add logic to handle reservations
        }
    }
}

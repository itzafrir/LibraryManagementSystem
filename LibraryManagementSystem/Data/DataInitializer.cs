using System;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraryManagementSystem.Data
{
    public static class DataInitializer
    {
        public static void Initialize(IRepository<Item> itemRepository, IRepository<User> userRepository, IRepository<Loan> loanRepository, IRepository<Review> reviewRepository, IRepository<Fine> fineRepository, IRepository<LoanRequest> loanRequestRepository)
        {
            // Ensure users are loaded from repository
            var users = userRepository.GetAll().ToList();

            // Ensure items are loaded from repository
            var items = itemRepository.GetAll().ToList();

            // Remove all loans and fines
            // Ensure loans and fines are reset
            //var loans = loanRepository.GetAll().ToList();
            //var fines = fineRepository.GetAll().ToList();
            //foreach (var loan in loans)
            //{
            //    loanRepository.Delete(loan.Id);
            //}

            //foreach (var fine in fines)
            //{
            //    fineRepository.Delete(fine.Id);
            //}

            // Create a fake loan request
            //var user1 = users.FirstOrDefault();  // Select the first user from the database
            //var item1 = items.FirstOrDefault();  // Select the first item from the database

            //if (user1 != null && item1 != null)
            //{
            //    var loanRequest = new LoanRequest
            //    {
            //        UserId = user1.Id,
            //        ItemId = item1.Id,
            //        RequestDate = DateTime.Now
            //    };

            //    loanRequestRepository.Add(loanRequest);
            //}

            // Update users to remove current loans and fines
            foreach (var user in users)
            {
                //user.CurrentLoans.Clear();
                //user.Fines.Clear();
                userRepository.Update(user);
            }

            // Associate reviews with items and update itemRepository
            foreach (var item in items)
            {
                item.Reviews = new ObservableCollection<Review>(reviewRepository.GetAll().Where(r => r.ItemId == item.Id).ToList());
                itemRepository.Update(item);
            }

            // Update the repositories with the new associations
            foreach (var item in items)
            {
                itemRepository.Update(item);
            }

            foreach (var user in users)
            {
                userRepository.Update(user);
            }
        }
    }
}

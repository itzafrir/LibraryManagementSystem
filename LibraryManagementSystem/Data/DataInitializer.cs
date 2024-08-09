using System;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Data
{
    public static class DataInitializer
    {
        public static void Initialize(IRepository<Item> itemRepository, IRepository<User> userRepository, IRepository<Loan> loanRepository, IRepository<Review> reviewRepository, IRepository<Fine> fineRepository, IRepository<LoanRequest> loanRequestRepository, IRepository<FinePayRequest> finePayRequestRepository)
        {
            // Fetch and ensure users are loaded
            var users = userRepository.GetAll().ToList();
            _ = users.Count; // Ensure data is fetched by accessing Count

            // Fetch and ensure items are loaded
            var items = itemRepository.GetAll().ToList();
            _ = items.Count; // Ensure data is fetched by accessing Count

            // Fetch and ensure loans are loaded
            var loans = loanRepository.GetAll().ToList();
            _ = loans.Count; // Ensure data is fetched by accessing Count

            // Fetch and ensure reviews are loaded
            var reviews = reviewRepository.GetAll().ToList();
            _ = reviews.Count; // Ensure data is fetched by accessing Count

            // Fetch and ensure fines are loaded
            var fines = fineRepository.GetAll().ToList();
            _ = fines.Count; // Ensure data is fetched by accessing Count

            // Fetch and ensure loan requests are loaded
            var loanRequests = loanRequestRepository.GetAll().ToList();
            _ = loanRequests.Count; // Ensure data is fetched by accessing Count

            return;

            # region Create a sample Fine
            var firstUser = users.FirstOrDefault();
            var firstItem = items.FirstOrDefault();

            if (firstUser != null && firstItem != null)
            {
                var fine = new Fine
                {
                    UserId = firstUser.Id,
                    ItemId = firstItem.Id,
                    Amount = 50.0, // Sample fine amount
                    DateIssued = DateTime.Now.AddDays(-5),
                    Status = FineStatus.Unpaid
                };

                fineRepository.Add(fine);

                // Create a sample FinePayRequest
                var finePayRequest = new FinePayRequest
                {
                    FineId = fine.Id,
                    UserId = firstUser.Id,
                    RequestDate = DateTime.Now
                };

                finePayRequestRepository.Add(finePayRequest);
            }
            #endregion
        }
    }
}
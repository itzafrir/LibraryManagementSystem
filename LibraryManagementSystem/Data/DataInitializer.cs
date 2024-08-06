using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Data
{
    public static class DataInitializer
    {
        public static void Initialize(IRepository<Item> itemRepository, IRepository<User> userRepository, IRepository<Loan> loanRepository, IRepository<Review> reviewRepository, IRepository<Fine> fineRepository, IRepository<LoanRequest> loanRequestRepository)
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
        }
    }
}
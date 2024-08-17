using System;
using System.Linq;
using System.Collections.Generic;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Data
{
    public static class DataInitializer
    {
        public static void Initialize(
            IRepository<Item> itemRepository,
            IRepository<User> userRepository,
            IRepository<Loan> loanRepository,
            IRepository<Review> reviewRepository,
            IRepository<Fine> fineRepository,
            IRepository<LoanRequest> loanRequestRepository,
            IRepository<FinePayRequest> finePayRequestRepository)
        {
            // Ensure the database is initialized with existing data
            var users = userRepository.GetAll().ToList();
            var items = itemRepository.GetAll().ToList();
            var loans = loanRepository.GetAll().ToList();
            var reviews = reviewRepository.GetAll().ToList();
            var fines = fineRepository.GetAll().ToList();
            var loanRequests = loanRequestRepository.GetAll().ToList();
        }
    }
}

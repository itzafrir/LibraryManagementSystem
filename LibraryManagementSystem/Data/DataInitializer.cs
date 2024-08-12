using System;
using System.Linq;
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

            //// Create a new book item
            //var book = new Book
            //{
            //    Title = "1984",
            //    Author = "George Orwell",
            //    Genre = "Dystopian",
            //    PageCount = 328,
            //    Language = "English",
            //    Format = "Paperback",
            //    Dimensions = "5 x 8 inches",
            //    Description = "A dystopian social science fiction novel.",
            //    Series = "null",
            //    Edition = 1,
            //    Keywords = new System.Collections.Generic.List<string> { "Dystopia", "Totalitarianism", "Classic" },
            //    PublicationDate = new DateTime(1949, 6, 8),
            //    Publisher = "Secker & Warburg",
            //    TotalCopies = 3,
            //    AvailableCopies = 3,
            //};

            //book.GenerateISBN();
            //itemRepository.Add(book);

            //// Create a new user
            //var user = new User
            //{
            //    Username = "c",
            //    Password = "c",
            //    FullName = "John Doe",
            //    Email = "johndoe@example.com",
            //    Address = "123 Main St",
            //    PhoneNumber = "123-456-7890",
            //    UserType = UserType.Librarian,
            //    MembershipDate = DateTime.Now.AddYears(-1)
            //};
            //userRepository.Add(user);

            //// Create a loan for the new book that is 2 months overdue
            //var loan = new Loan
            //{
            //    ItemId = book.Id,
            //    UserId = user.Id,
            //    LoanDate = DateTime.Now.AddMonths(-3), // Loaned 3 months ago
            //    DueDate = DateTime.Now.AddMonths(-2),  // Due 2 months ago
            //    LoanStatus = LoanStatus.Active
            //};
            //loanRepository.Add(loan);

            ////// Mark the loan as overdue and create a fine
            ////if (loan.IsOverdue())
            ////{
            ////    var fine = new Fine
            ////    {
            ////        UserId = user.Id,
            ////        ItemId = book.Id,
            ////        Amount = 2 * 1.0, // Assuming $1 per month overdue
            ////        DateIssued = DateTime.Now,
            ////        Status = FineStatus.Unpaid
            ////    };
            ////    fineRepository.Add(fine);
            ////}

            //// Update the user's current loans
            //user.CurrentLoans.Add(loan);
            //userRepository.Update(user);

            //// Decrement available copies of the book
            //book.AvailableCopies--;
            //itemRepository.Update(book);
        }
    }
}

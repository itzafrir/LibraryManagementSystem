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
            _ = fines.Count; // Ensure data is fetched by accessing CountR

            // Fetch and ensure loan requests are loaded
            var loanRequests = loanRequestRepository.GetAll().ToList();
            _ = loanRequests.Count; // Ensure data is fetched by accessing Count

            //// Update the third && fourth users to have 0 loans and 0 loan requests
            //if (users.Count >= 4)
            //{
            //    var thirdUser = users[2];

            //    // Remove loans associated with the third user
            //    var userLoans = loans.Where(l => l.UserId == thirdUser.Id).ToList();
            //    foreach (var loan in userLoans)
            //    {
            //        loanRepository.Delete(loan.Id);
            //    }

            //    // Remove loan requests associated with the third user
            //    var userLoanRequests = loanRequests.Where(lr => lr.UserId == thirdUser.Id).ToList();
            //    foreach (var loanRequest in userLoanRequests)
            //    {
            //        loanRequestRepository.Delete(loanRequest.Id);
            //    }

            //    // Clear the user's loan and loan request lists
            //    thirdUser.CurrentLoans.Clear();
            //    thirdUser.LoanRequests.Clear();

            //    // Update the user in the repository
            //    userRepository.Update(thirdUser);

            //    thirdUser = users[3];

            //    // Remove loans associated with the third user
            //    userLoans = loans.Where(l => l.UserId == thirdUser.Id).ToList();
            //    foreach (var loan in userLoans)
            //    {
            //        loanRepository.Delete(loan.Id);
            //    }

            //    // Remove loan requests associated with the third user
            //    userLoanRequests = loanRequests.Where(lr => lr.UserId == thirdUser.Id).ToList();
            //    foreach (var loanRequest in userLoanRequests)
            //    {
            //        loanRequestRepository.Delete(loanRequest.Id);
            //    }

            //    // Clear the user's loan and loan request lists
            //    thirdUser.CurrentLoans.Clear();
            //    thirdUser.LoanRequests.Clear();

            //    // Update the user in the repository
            //    userRepository.Update(thirdUser);


            //}

            //return;

            //# region Create a sample Fine
            //var firstUser = users.FirstOrDefault();
            //var firstItem = items.FirstOrDefault();

            //if (firstUser != null && firstItem != null)
            //{
            //    var fine = new Fine
            //    {
            //        UserId = firstUser.Id,
            //        ItemId = firstItem.Id,
            //        Amount = 50.0, // Sample fine amount
            //        DateIssued = DateTime.Now.AddDays(-5),
            //        Status = FineStatus.Unpaid
            //    };

            //    fineRepository.Add(fine);

            //    // Create a sample FinePayRequest
            //    var finePayRequest = new FinePayRequest
            //    {
            //        FineId = fine.Id,
            //        UserId = firstUser.Id,
            //        RequestDate = DateTime.Now
            //    };

            //    finePayRequestRepository.Add(finePayRequest);
            //}
            //#endregion
        }
    }
}
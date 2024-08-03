using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraryManagementSystem.Data
{
    public static class DataInitializer
    {
        public static void Initialize(IRepository<Item> itemRepository, IRepository<User> userRepository, IRepository<Loan> loanRepository, IRepository<Review> reviewRepository)
        {
            // Ensure users are loaded from repository
            var users = userRepository.GetAll().ToList();

            // Ensure items are loaded from repository
            var items = itemRepository.GetAll().ToList();

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

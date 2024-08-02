using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Data
{
    public static class DataInitializer
    {
        public static void Initialize(IRepository<Item> itemRepository, IRepository<User> userRepository, IRepository<Loan> loanRepository, IRepository<Review> reviewRepository)
        {
            var items = new List<Item>
            {
                new Book
                {
                    Title = "The Great Gatsby",
                    ISBN = "9780743273565",
                    ItemType = ItemType.Book,
                    Rating = 4.5,
                    PublicationDate = new DateTime(1925, 4, 10),
                    Publisher = "Charles Scribner's Sons",
                    Description = "A novel set in the Roaring Twenties.",
                    Author = "F. Scott Fitzgerald",
                },
                new EBook
                {
                    Title = "1984",
                    ISBN = "9780451524935",
                    ItemType = ItemType.EBook,
                    Rating = 4.8,
                    PublicationDate = new DateTime(1949, 6, 8),
                    Publisher = "Secker & Warburg",
                    Description = "A dystopian social science fiction novel and cautionary tale.",
                    Author = "George Orwell"
                },
            };

            foreach (var item in items)
            {
                if (!itemRepository.GetAll().Any(i => i.Title == item.Title && i.ISBN == item.ISBN))
                {
                    itemRepository.Add(item);
                }
            }

            var users = new List<User>
            {
                new User
                {
                    Username = "john_doe",
                    Password = "password123",
                    FullName = "John Doe",
                    Email = "john@example.com",
                    Address = "123 Main St",
                    PhoneNumber = "555-1234",
                    UserType = UserType.Member,
                    MembershipDate = DateTime.Now.AddYears(-2),
                },
                new User
                {
                    Username = "a",
                    Password = "a",
                    FullName = "John Doe",
                    Email = "i@example.com",
                    Address = "1234 Main St",
                    PhoneNumber = "555-122224",
                    UserType = UserType.Librarian,
                    MembershipDate = DateTime.Now.AddYears(-2),
                },
                new User
                {
                    Username = "jane_smith",
                    Password = "password123",
                    FullName = "Jane Smith",
                    Email = "jane@example.com",
                    Address = "456 Elm St",
                    PhoneNumber = "555-5678",
                    UserType = UserType.Librarian,
                    MembershipDate = DateTime.Now.AddYears(-3),
                },
            };

            foreach (var user in users)
            {
                if (userRepository.GetAll().All(u => u.Username != user.Username))
                {
                    userRepository.Add(user);
                }
            }

            // Clear existing reviews
            //var existingReviews = reviewRepository.GetAll().ToList();
            //foreach (var review in existingReviews)
            //{
            //    reviewRepository.Delete(review.Id);
            //}

            //var reviews = new List<Review>
            //{
            //    new Review
            //    {
            //        ItemId = itemRepository.GetAll().First(i => i.Title == "The Great Gatsby").Id,
            //        UserId = userRepository.GetAll().First(u => u.Username == "john_doe").Id,
            //        Rating = 5,
            //        Text = "Amazing book!",
            //        ReviewDate = DateTime.Now.AddDays(-2)
            //    },
            //    new Review
            //    {
            //        ItemId = itemRepository.GetAll().First(i => i.Title == "1984").Id,
            //        UserId = userRepository.GetAll().First(u => u.Username == "jane_smith").Id,
            //        Rating = 4,
            //        Text = "Very thought-provoking.",
            //        ReviewDate = DateTime.Now.AddDays(-1)
            //    },
            //};

            //foreach (var review in reviews)
            //{
            //    if (!reviewRepository.GetAll().Any(r => r.ItemId == review.ItemId && r.UserId == review.UserId))
            //    {
            //        reviewRepository.Add(review);
            //    }
            //}
        }
    }
}

﻿using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public static class DataInitializer
    {
        public static void Initialize(
            IRepository<Item> itemRepository,
            IRepository<User> userRepository,
            IRepository<Loan> loanRepository,
            IRepository<Review> reviewRepository,
            IRepository<Fine> fineRepository)
        {
            // Seed items
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
                    TotalCopies = 5,
                    AvailableCopies = 5
                },
                new CD
                {
                    Title = "Abbey Road",
                    ISBN = "B0025KVLU8",
                    ItemType = ItemType.CD,
                    Rating = 4.9,
                    PublicationDate = new DateTime(1969, 9, 26),
                    Publisher = "Apple Records",
                    Description = "The Beatles' eleventh studio album.",
                    Artist = "The Beatles",
                    TotalCopies = 3,
                    AvailableCopies = 3
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
                    Author = "George Orwell",
                    TotalCopies = 10,
                    AvailableCopies = 10
                },
            };

            foreach (var item in items)
            {
                if (!itemRepository.GetAll().Any(i => i.Title == item.Title && i.ISBN == item.ISBN))
                {
                    itemRepository.Add(item);
                }
            }

            // Seed users
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

            // Seed loans
            var loans = new List<Loan>
            {
                new Loan
                {
                    ItemId = itemRepository.GetAll().First(i => i.Title == "The Great Gatsby").Id,
                    UserId = userRepository.GetAll().First(u => u.Username == "john_doe").Id,
                    LoanDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(4),
                    LoanStatus = LoanStatus.Active
                },
                new Loan
                {
                    ItemId = itemRepository.GetAll().First(i => i.Title == "Abbey Road").Id,
                    UserId = userRepository.GetAll().First(u => u.Username == "jane_smith").Id,
                    LoanDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(9),
                    LoanStatus = LoanStatus.Active
                },
            };

            foreach (var loan in loans)
            {
                if (!loanRepository.GetAll().Any(l => l.ItemId == loan.ItemId && l.UserId == loan.UserId && l.LoanDate == loan.LoanDate))
                {
                    loanRepository.Add(loan);

                    var item = itemRepository.GetById(loan.ItemId);
                    item.AvailableCopies--;
                    itemRepository.Update(item);
                }
            }

            // Seed reviews
            var reviews = new List<Review>
            {
                new Review
                {
                    ItemId = itemRepository.GetAll().First(i => i.Title == "The Great Gatsby").Id,
                    UserId = userRepository.GetAll().First(u => u.Username == "john_doe").Id,
                    Rating = 5,
                    Text = "Amazing book!",
                    ReviewDate = DateTime.Now.AddDays(-2)
                },
                new Review
                {
                    ItemId = itemRepository.GetAll().First(i => i.Title == "1984").Id,
                    UserId = userRepository.GetAll().First(u => u.Username == "jane_smith").Id,
                    Rating = 4,
                    Text = "Very thought-provoking.",
                    ReviewDate = DateTime.Now.AddDays(-1)
                },
            };

            foreach (var review in reviews)
            {
                if (!reviewRepository.GetAll().Any(r => r.ItemId == review.ItemId && r.UserId == review.UserId && r.ReviewDate == review.ReviewDate))
                {
                    reviewRepository.Add(review);
                }
            }

            // Seed fines
            var fines = new List<Fine>
            {
                new Fine
                {
                    UserId = userRepository.GetAll().First(u => u.Username == "john_doe").Id,
                    Amount = 10.0,
                    DateIssued = DateTime.Now.AddDays(-30),
                    DatePaid = null
                },
                new Fine
                {
                    UserId = userRepository.GetAll().First(u => u.Username == "jane_smith").Id,
                    Amount = 5.0,
                    DateIssued = DateTime.Now.AddDays(-15),
                    DatePaid = DateTime.Now.AddDays(-5)
                }
            };

            foreach (var fine in fines)
            {
                if (!fineRepository.GetAll().Any(f => f.UserId == fine.UserId && f.DateIssued == fine.DateIssued))
                {
                    fineRepository.Add(fine);
                }
            }
        }
    }
}

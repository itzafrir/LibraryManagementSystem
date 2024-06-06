using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Utilities.Enums;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem
{
    public static class DataInitializer
    {
        public static void Initialize(IRepository<Item> itemRepository, IRepository<User> userRepository, IRepository<Loan> loanRepository)
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
                    // EBooks don't need CopiesByLocation
                },
                // Add more items as needed
            };

            foreach (var item in items)
            {
                itemRepository.Add(item);
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
                    Fines = 0.0
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
                    Fines = 5.0
                },
                // Add more users as needed
            };

            foreach (var user in users)
            {
                userRepository.Add(user);
            }

            var loans = new List<Loan>
            {
                new Loan
                {
                    ItemId = 1,
                    UserId = 1,
                    LoanDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(4),
                    LoanStatus = LoanStatus.Active
                },
                new Loan
                {
                    ItemId = 2,
                    UserId = 2,
                    LoanDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(9),
                    LoanStatus = LoanStatus.Active
                },
                // Add more loans as needed
            };

            foreach (var loan in loans)
            {
                loanRepository.Add(loan);
            }
        }
    }
}

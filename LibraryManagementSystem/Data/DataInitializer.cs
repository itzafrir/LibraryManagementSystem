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
            // Initialize sample items
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
                    Genre = "Fiction",
                    PageCount = 180,
                    Language = "English",
                    Format = "Hardcover",
                    Dimensions = "5.5 x 0.8 x 8.2 inches",
                    Series = "",
                    Edition = 1,
                    Keywords = new List<string> { "classic", "1920s", "American literature" },
                    CopiesByLocations = new List<CopiesByLocation>
                    {
                        new CopiesByLocation { Location = "Main Library", Copies = 3 },
                        new CopiesByLocation { Location = "East Branch", Copies = 2 }
                    }
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
                    Genre = "Rock",
                    Duration = new TimeSpan(0, 47, 23),
                    TrackCount = 17,
                    Label = "Apple",
                    ReleaseDate = new DateTime(1969, 9, 26),
                    Tracks = new List<string> { "Come Together", "Something", "Maxwell's Silver Hammer" },
                    CopiesByLocations = new List<CopiesByLocation>
                    {
                        new CopiesByLocation { Location = "Main Library", Copies = 4 },
                        new CopiesByLocation { Location = "West Branch", Copies = 3 }
                    }
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
                    FileFormat = "EPUB",
                    FileSize = 2.5,
                    DownloadLink = "http://example.com/1984.epub",
                    Keywords = new List<string> { "dystopia", "totalitarianism", "classic" }
                },
                // Add more items as needed
            };

            foreach (var item in items)
            {
                itemRepository.Add(item);
            }

            // Initialize sample users
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

            // Initialize sample loans
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

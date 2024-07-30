using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Services
{
    public class UserService
    {
        private readonly List<User> _users;
        private User _currentUser;

        public UserService()
        {
            // Dummy data for demonstration purposes
            _users = new List<User>
            {
                new User { Id = 1, Username = "john_doe", Password = "d",FullName = "John Doe", Email = "john@example.com", UserType = UserType.Member, MembershipDate = DateTime.Now.AddYears(-2), Fines = 0.0 },
                new User { Id = 2, Username = "jane_smith",Password = "d", FullName = "Jane Smith", Email = "jane@example.com", UserType = UserType.Librarian, MembershipDate = DateTime.Now.AddYears(-3), Fines = 5.0 },
                new User { Id = 3, Username = "a",Password = "a",FullName = "Sam Green", Email = "sam@example.com", UserType = UserType.Guest, MembershipDate = DateTime.Now.AddMonths(-6), Fines = 10.0 },
                // Add more users as needed
            };
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public void RemoveUser(User user)
        {
            var userToRemove = _users.FirstOrDefault(u => u.Id == user.Id);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
            }
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public List<Loan> GetCurrentLoans()
        {
            // Retrieve loans for the current user
            return new List<Loan>
            {
                new Loan { Id = 1, ItemId = 1, UserId = _currentUser.Id, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(4), LoanStatus = LoanStatus.Active }
            };
        }

        public List<Item> GetOrders()
        {
            // Retrieve orders for the current user
            return new List<Item>
            {
                new Book { Id = 1, Title = "The Great Gatsby", ISBN = "9780743273565", ItemType = ItemType.Book, Rating = 4.5, PublicationDate = new DateTime(1925, 4, 10), Publisher = "Charles Scribner's Sons", Description = "A novel set in the Roaring Twenties.", Author = "F. Scott Fitzgerald" }
            };
        }

        public List<Fine> GetFines()
        {
            // Retrieve fines for the current user
            return new List<Fine>
            {
                new Fine { Id = 1, UserId = _currentUser.Id, Amount = 5.0, DateIssued = DateTime.Now.AddDays(-30), DatePaid = null }
            };
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public User? ValidateUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null && user.Password == password)
            {
                _currentUser = user;
                return user;
            }
            return null;
        }

        public bool IsUserLoggedIn()
        {
            return _currentUser != null;
        }
    }
}

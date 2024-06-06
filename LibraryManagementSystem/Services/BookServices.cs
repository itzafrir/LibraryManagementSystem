using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        public IEnumerable<Book> GetAllBooks()
        {
            using (var context = new LibraryContext())
            {
                return context.Books.ToList();
            }
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            using (var context = new LibraryContext())
            {
                return context.Books
                    .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm) || b.ISBN.Contains(searchTerm))
                    .ToList();
            }
        }

        public Book GetBookById(int id)
        {
            using (var context = new LibraryContext())
            {
                return context.Books.Find(id);
            }
        }

        public void AddBook(Book book)
        {
            using (var context = new LibraryContext())
            {
                context.Books.Add(book);
                context.SaveChanges();
            }
        }

        public void UpdateBook(Book book)
        {
            using (var context = new LibraryContext())
            {
                context.Books.Update(book);
                context.SaveChanges();
            }
        }

        public void DeleteBook(int id)
        {
            using (var context = new LibraryContext())
            {
                var book = context.Books.Find(id);
                if (book != null)
                {
                    context.Books.Remove(book);
                    context.SaveChanges();
                }
            }
        }
    }
}
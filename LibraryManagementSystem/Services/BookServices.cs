using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public Book GetBookById(int id)
        {
            return _bookRepository.GetById(id);
        }

        public void AddBook(Book book)
        {
            _bookRepository.Add(book);
        }

        public void UpdateBook(Book book)
        {
            _bookRepository.Update(book);
        }

        public void DeleteBook(int id)
        {
            _bookRepository.Delete(id);
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _bookRepository.GetAll()
                .Where(book => book.Title.Contains(searchTerm) || book.Author.Contains(searchTerm));
        }
    }
}
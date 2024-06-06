using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    public class UserViewModel : ObservableObject
    {
        private readonly BookService _bookService;
        private Book _selectedBook;
        private string _searchTerm;
        private UserType _userType;

        public ObservableCollection<Book> Books { get; private set; }

        public Book SelectedBook
        {
            get => _selectedBook;
            set => SetProperty(ref _selectedBook, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public UserType UserType
        {
            get => _userType;
            set => SetProperty(ref _userType, value);
        }

        public IRelayCommand SearchCommand { get; private set; }
        public IRelayCommand AddBookCommand { get; private set; }
        public IRelayCommand UpdateBookCommand { get; private set; }
        public IRelayCommand DeleteBookCommand { get; private set; }

        public UserViewModel(UserType userType)
        {
            _bookService = new BookService();
            UserType = userType;

            Books = new ObservableCollection<Book>(_bookService.GetAllBooks());

            SearchCommand = new RelayCommand(SearchBooks);
            AddBookCommand = new RelayCommand(AddBook, CanExecuteAdminCommands);
            UpdateBookCommand = new RelayCommand(UpdateBook, CanExecuteAdminCommands);
            DeleteBookCommand = new RelayCommand(DeleteBook, CanExecuteAdminCommands);
        }

        private void SearchBooks()
        {
            Books.Clear();
            var searchResults = _bookService.SearchBooks(SearchTerm);
            foreach (var book in searchResults)
            {
                Books.Add(book);
            }
        }

        private bool CanExecuteAdminCommands()
        {
            return UserType == UserType.Librarian || UserType == UserType.Manager || UserType == UserType.Assistant;
        }

        private void AddBook()
        {
            var newBook = new Book
            {
                Title = "New Book",
                Author = "New Author",
                ISBN = "New ISBN"
            };
            _bookService.AddBook(newBook);
            Books.Add(newBook);
            MessageBox.Show("Book added successfully.", "Success");
        }

        private void UpdateBook()
        {
            if (SelectedBook != null)
            {
                _bookService.UpdateBook(SelectedBook);
                MessageBox.Show("Book updated successfully.", "Success");
            }
        }

        private void DeleteBook()
        {
            if (SelectedBook != null)
            {
                _bookService.DeleteBook(SelectedBook.Id);
                Books.Remove(SelectedBook);
                SelectedBook = null;
                MessageBox.Show("Book deleted successfully.", "Success");
            }
        }
    }
}

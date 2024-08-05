using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class BookView : Window
    {
        private readonly ItemService _itemService;
        private Book _book;

        public BookView(Book book, ItemService itemService)
        {
            InitializeComponent();
            _itemService = itemService;
            _book = book;
            DataContext = _book;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (_book.Id == 0)
                {
                    _book.ISBN = _book.GenerateISBN();
                    _itemService.AddItem(_book);
                }
                else
                {
                    _itemService.UpdateItem(_book);
                }
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_book.Title) &&
                   !string.IsNullOrWhiteSpace(_book.Author) &&
                   !string.IsNullOrWhiteSpace(_book.Genre) &&
                   _book.PageCount > 0 &&
                   !string.IsNullOrWhiteSpace(_book.Language) &&
                   !string.IsNullOrWhiteSpace(_book.Format) &&
                   !string.IsNullOrWhiteSpace(_book.Dimensions) &&
                   !string.IsNullOrWhiteSpace(_book.Series) &&
                   _book.Edition > 0 &&
                   !string.IsNullOrWhiteSpace(_book.Publisher) &&
                   _book.PublicationDate != default &&
                   !string.IsNullOrWhiteSpace(_book.Description);
        }
    }
}

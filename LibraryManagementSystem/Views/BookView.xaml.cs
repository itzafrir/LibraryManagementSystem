using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class BookView : Window
    {
        public BookView(Book book, ItemService itemService)
        {
            InitializeComponent();
            DataContext = new BookViewModel(book, itemService, Close);
        }
    }
}
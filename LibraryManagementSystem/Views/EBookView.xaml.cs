using System;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class EBookView : Window
    {
        private readonly ItemService _itemService;
        private EBook _eBook;

        public EBookView(EBook eBook, ItemService itemService)
        {
            InitializeComponent();
            _itemService = itemService;
            _eBook = eBook;
            DataContext = _eBook;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (_eBook.Id == 0)
                {
                    _itemService.AddItem(_eBook);
                }
                else
                {
                    _itemService.UpdateItem(_eBook);
                }
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields and ensure that Total Copies is greater than or equal to Available Copies.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_eBook.Title) &&
                   !string.IsNullOrWhiteSpace(_eBook.Author) &&
                   !string.IsNullOrWhiteSpace(_eBook.FileFormat) &&
                   _eBook.FileSize > 0 &&
                   !string.IsNullOrWhiteSpace(_eBook.DownloadLink) &&
                   _eBook.TotalCopies > 0 &&
                   _eBook.AvailableCopies >= 0 &&
                   _eBook.TotalCopies >= _eBook.AvailableCopies;
        }
    }
}
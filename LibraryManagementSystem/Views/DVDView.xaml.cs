using System;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class DVDView : Window
    {
        private readonly ItemService _itemService;
        private DVD _dvd;

        public DVDView(DVD dvd, ItemService itemService)
        {
            InitializeComponent();
            _itemService = itemService;
            _dvd = dvd;
            DataContext = _dvd;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (_dvd.Id == 0)
                {
                    _itemService.AddItem(_dvd);
                }
                else
                {
                    _itemService.UpdateItem(_dvd);
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
            return !string.IsNullOrWhiteSpace(_dvd.Title) &&
                   !string.IsNullOrWhiteSpace(_dvd.Director) &&
                   !string.IsNullOrWhiteSpace(_dvd.Genre) &&
                   _dvd.Duration != default &&
                   !string.IsNullOrWhiteSpace(_dvd.Language) &&
                   !string.IsNullOrWhiteSpace(_dvd.Studio) &&
                   _dvd.TotalCopies > 0 &&
                   _dvd.AvailableCopies >= 0 &&
                   _dvd.TotalCopies >= _dvd.AvailableCopies;
        }
    }
}
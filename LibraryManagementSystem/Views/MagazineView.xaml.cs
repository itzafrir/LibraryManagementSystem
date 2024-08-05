using System;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class MagazineView : Window
    {
        private readonly ItemService _itemService;
        private Magazine _magazine;

        public MagazineView(Magazine magazine, ItemService itemService)
        {
            InitializeComponent();
            _itemService = itemService;
            _magazine = magazine;
            DataContext = _magazine;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (_magazine.Id == 0)
                {
                    _itemService.AddItem(_magazine);
                }
                else
                {
                    _itemService.UpdateItem(_magazine);
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
            return !string.IsNullOrWhiteSpace(_magazine.Title) &&
                   !string.IsNullOrWhiteSpace(_magazine.Editor) &&
                   _magazine.IssueNumber > 0 &&
                   !string.IsNullOrWhiteSpace(_magazine.Genre) &&
                   !string.IsNullOrWhiteSpace(_magazine.Frequency) &&
                   _magazine.TotalCopies > 0 &&
                   _magazine.AvailableCopies >= 0 &&
                   _magazine.TotalCopies >= _magazine.AvailableCopies;
        }
    }
}
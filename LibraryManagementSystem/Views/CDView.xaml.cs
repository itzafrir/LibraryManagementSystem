using System;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    public partial class CDView : Window
    {
        private readonly ItemService _itemService;
        private CD _cd;

        public CDView(CD cd, ItemService itemService)
        {
            InitializeComponent();
            _itemService = itemService;
            _cd = cd;
            DataContext = _cd;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (_cd.Id == 0)
                {
                    _itemService.AddItem(_cd);
                }
                else
                {
                    _itemService.UpdateItem(_cd);
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
            return !string.IsNullOrWhiteSpace(_cd.Title) &&
                   !string.IsNullOrWhiteSpace(_cd.Artist) &&
                   !string.IsNullOrWhiteSpace(_cd.Genre) &&
                   _cd.TrackCount > 0 &&
                   !string.IsNullOrWhiteSpace(_cd.Label) &&
                   _cd.TotalCopies > 0 &&
                   _cd.AvailableCopies >= 0 &&
                   _cd.TotalCopies >= _cd.AvailableCopies;
        }
    }
}
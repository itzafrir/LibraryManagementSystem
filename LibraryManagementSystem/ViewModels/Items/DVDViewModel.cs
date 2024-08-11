using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class DVDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        public DVD DVD { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public DVDViewModel(DVD dvd, ItemService itemService, Action closeAction)
        {
            DVD = dvd ?? new DVD();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            // Step 1: Validate the data
            if (!ValidateDVD())
            {
                return; // Prevent saving if validation fails
            }

            // Step 2: Update the copies if needed (handled internally by the TrySetTotalCopies method)
            bool success = DVD.TrySetTotalCopies(DVD.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            // Step 3: Save or update the item
            if (DVD.Id == 0)
            {
                _itemService.AddItem(DVD);
            }
            else
            {
                _itemService.UpdateItem(DVD);
            }

            // Step 4: Close the view
            _closeAction();
        }

        private bool ValidateDVD()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(DVD);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific DVD properties
            if (string.IsNullOrWhiteSpace(DVD.Director))
            {
                errors.Add("Director cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(DVD.Genre))
            {
                errors.Add("Genre cannot be empty.");
            }

            if (DVD.Duration == TimeSpan.Zero)
            {
                errors.Add("Duration must be a valid non-zero time.");
            }

            if (string.IsNullOrWhiteSpace(DVD.Language))
            {
                errors.Add("Language cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(DVD.Studio))
            {
                errors.Add("Studio cannot be empty.");
            }

            if (DVD.ReleaseDate == DateTime.MinValue)
            {
                errors.Add("Release Date must be a valid date.");
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Validation passed
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}

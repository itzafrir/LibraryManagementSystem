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
    /// <summary>
    /// ViewModel for managing the creation and editing of DVD items within the library management system.
    /// </summary>
    public class DVDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        /// <summary>
        /// Gets the DVD item being created or edited.
        /// </summary>
        public DVD DVD { get; }

        /// <summary>
        /// Command to save the DVD item to the system.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to cancel the operation and close the view.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DVDViewModel"/> class.
        /// </summary>
        /// <param name="dvd">The DVD item being managed. If null, a new DVD will be created.</param>
        /// <param name="itemService">Service for managing item operations.</param>
        /// <param name="closeAction">Action to execute when the view should be closed.</param>
        public DVDViewModel(DVD dvd, ItemService itemService, Action closeAction)
        {
            DVD = dvd ?? new DVD();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Validates the DVD's properties and saves it to the system.
        /// </summary>
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

        /// <summary>
        /// Validates the properties of the DVD to ensure they meet the required criteria.
        /// </summary>
        /// <returns>True if the DVD is valid; otherwise, false.</returns>
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

            // Show validation errors if any
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Validation passed
        }

        /// <summary>
        /// Cancels the operation and closes the view without saving any changes.
        /// </summary>
        private void Cancel()
        {
            _closeAction();
        }
    }
}

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
    /// ViewModel for managing the creation and editing of CD items within the library management system.
    /// </summary>
    public class CDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        /// <summary>
        /// Gets the CD item being created or edited.
        /// </summary>
        public CD CD { get; }

        /// <summary>
        /// Command to save the CD item to the system.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to cancel the operation and close the view.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CDViewModel"/> class.
        /// </summary>
        /// <param name="cd">The CD item being managed. If null, a new CD will be created.</param>
        /// <param name="itemService">Service for managing item operations.</param>
        /// <param name="closeAction">Action to execute when the view should be closed.</param>
        public CDViewModel(CD cd, ItemService itemService, Action closeAction)
        {
            CD = cd ?? new CD();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Validates the CD's properties and saves it to the system.
        /// </summary>
        private void Save()
        {
            // Step 1: Validate the data
            if (!ValidateCD())
            {
                return; // Prevent saving if validation fails
            }

            // Step 2: Update the copies if needed (handled internally by the TrySetTotalCopies method)
            bool success = CD.TrySetTotalCopies(CD.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            // Step 3: Save or update the item
            if (CD.Id == 0)
            {
                _itemService.AddItem(CD);
            }
            else
            {
                _itemService.UpdateItem(CD);
            }

            // Step 4: Close the view
            _closeAction();
        }

        /// <summary>
        /// Validates the properties of the CD to ensure they meet the required criteria.
        /// </summary>
        /// <returns>True if the CD is valid; otherwise, false.</returns>
        private bool ValidateCD()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(CD);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific CD properties
            if (string.IsNullOrWhiteSpace(CD.Artist))
            {
                errors.Add("Artist cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(CD.Genre))
            {
                errors.Add("Genre cannot be empty.");
            }

            if (CD.Duration == TimeSpan.Zero)
            {
                errors.Add("Duration must be a valid non-zero time.");
            }

            if (CD.TrackCount <= 0)
            {
                errors.Add("Track Count must be a positive number.");
            }

            if (string.IsNullOrWhiteSpace(CD.Label))
            {
                errors.Add("Label cannot be empty.");
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

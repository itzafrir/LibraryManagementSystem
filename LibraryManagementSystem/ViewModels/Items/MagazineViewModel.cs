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
    /// ViewModel for managing the creation and editing of magazine items within the library management system.
    /// </summary>
    public class MagazineViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        /// <summary>
        /// Gets the magazine item being created or edited.
        /// </summary>
        public Magazine Magazine { get; }

        /// <summary>
        /// Command to save the magazine item to the system.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to cancel the operation and close the view.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagazineViewModel"/> class.
        /// </summary>
        /// <param name="magazine">The magazine item being managed. If null, a new magazine will be created.</param>
        /// <param name="itemService">Service for managing item operations.</param>
        /// <param name="closeAction">Action to execute when the view should be closed.</param>
        public MagazineViewModel(Magazine magazine, ItemService itemService, Action closeAction)
        {
            Magazine = magazine ?? new Magazine();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Validates the magazine's properties and saves it to the system.
        /// </summary>
        private void Save()
        {
            // Step 1: Validate the data
            if (!ValidateMagazine())
            {
                return; // Prevent saving if validation fails
            }

            // Step 2: Update the copies if needed (handled internally by the TrySetTotalCopies method)
            bool success = Magazine.TrySetTotalCopies(Magazine.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            // Step 3: Save or update the item
            if (Magazine.Id == 0)
            {
                _itemService.AddItem(Magazine);
            }
            else
            {
                _itemService.UpdateItem(Magazine);
            }

            // Step 4: Close the view
            _closeAction();
        }

        /// <summary>
        /// Validates the properties of the magazine to ensure they meet the required criteria.
        /// </summary>
        /// <returns>True if the magazine is valid; otherwise, false.</returns>
        private bool ValidateMagazine()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(Magazine);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific magazine properties
            if (string.IsNullOrWhiteSpace(Magazine.Editor))
            {
                errors.Add("Editor cannot be empty.");
            }

            if (Magazine.IssueNumber <= 0)
            {
                errors.Add("Issue Number must be a positive number.");
            }

            if (string.IsNullOrWhiteSpace(Magazine.Genre))
            {
                errors.Add("Genre cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(Magazine.Frequency))
            {
                errors.Add("Frequency cannot be empty.");
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

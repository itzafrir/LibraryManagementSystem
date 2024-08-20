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
    /// ViewModel for managing the creation and editing of eBook items within the library management system.
    /// </summary>
    public class EBookViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        /// <summary>
        /// Gets the eBook item being created or edited.
        /// </summary>
        public EBook EBook { get; }

        /// <summary>
        /// Command to save the eBook item to the system.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to cancel the operation and close the view.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EBookViewModel"/> class.
        /// </summary>
        /// <param name="ebook">The eBook item being managed. If null, a new eBook will be created.</param>
        /// <param name="itemService">Service for managing item operations.</param>
        /// <param name="closeAction">Action to execute when the view should be closed.</param>
        public EBookViewModel(EBook ebook, ItemService itemService, Action closeAction)
        {
            EBook = ebook ?? new EBook();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Validates the eBook's properties and saves it to the system.
        /// </summary>
        private void Save()
        {
            // Step 1: Validate the data
            if (!ValidateEBook())
            {
                return; // Prevent saving if validation fails
            }

            // Step 2: Update the copies if needed (handled internally by the TrySetTotalCopies method)
            bool success = EBook.TrySetTotalCopies(EBook.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            // Step 3: Save or update the item
            if (EBook.Id == 0)
            {
                _itemService.AddItem(EBook);
            }
            else
            {
                _itemService.UpdateItem(EBook);
            }

            // Step 4: Close the view
            _closeAction();
        }

        /// <summary>
        /// Validates the properties of the eBook to ensure they meet the required criteria.
        /// </summary>
        /// <returns>True if the eBook is valid; otherwise, false.</returns>
        private bool ValidateEBook()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(EBook);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific eBook properties
            if (string.IsNullOrWhiteSpace(EBook.Author))
            {
                errors.Add("Author cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(EBook.FileFormat))
            {
                errors.Add("File Format cannot be empty.");
            }

            if (EBook.FileSize <= 0)
            {
                errors.Add("File Size must be a positive number.");
            }

            if (string.IsNullOrWhiteSpace(EBook.DownloadLink))
            {
                errors.Add("Download Link cannot be empty.");
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

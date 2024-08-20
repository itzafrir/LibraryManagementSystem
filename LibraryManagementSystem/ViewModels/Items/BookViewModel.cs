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
    /// ViewModel for managing the creation and editing of book items within the library management system.
    /// </summary>
    public class BookViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        /// <summary>
        /// Gets the book item being created or edited.
        /// </summary>
        public Book Book { get; }

        /// <summary>
        /// Command to save the book item to the system.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to cancel the operation and close the view.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookViewModel"/> class.
        /// </summary>
        /// <param name="book">The book item being managed. If null, a new book will be created.</param>
        /// <param name="itemService">Service for managing item operations.</param>
        /// <param name="closeAction">Action to execute when the view should be closed.</param>
        public BookViewModel(Book book, ItemService itemService, Action closeAction)
        {
            Book = book ?? new Book();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Validates the book's properties and saves it to the system.
        /// </summary>
        private void Save()
        {
            // Step 1: Validate the data
            if (!ValidateBook())
            {
                return; // Prevent saving if validation fails
            }

            // Step 2: Update the copies if needed (handled internally by the TrySetTotalCopies method)
            bool success = Book.TrySetTotalCopies(Book.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            // Step 3: Save or update the item
            if (Book.Id == 0)
            {
                _itemService.AddItem(Book);
            }
            else
            {
                _itemService.UpdateItem(Book);
            }

            // Step 4: Close the view
            _closeAction();
        }

        /// <summary>
        /// Cancels the operation and closes the view without saving any changes.
        /// </summary>
        private void Cancel()
        {
            _closeAction();
        }

        /// <summary>
        /// Validates the properties of the book to ensure they meet the required criteria.
        /// </summary>
        /// <returns>True if the book is valid; otherwise, false.</returns>
        private bool ValidateBook()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(Book);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific book properties
            if (string.IsNullOrWhiteSpace(Book.Author))
            {
                errors.Add("Author cannot be empty.");
            }

            if (!int.TryParse(Book.PageCount.ToString(), out _))
            {
                errors.Add("Page Count must be a valid number.");
            }

            if (string.IsNullOrWhiteSpace(Book.Genre))
            {
                errors.Add("Genre cannot be empty.");
            }

            // Show validation errors if any
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}

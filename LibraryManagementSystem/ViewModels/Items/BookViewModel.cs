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
    public class BookViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        public Book Book { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public BookViewModel(Book book, ItemService itemService, Action closeAction)
        {
            Book = book ?? new Book();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

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


        private void Cancel()
        {
            _closeAction();
        }
        private bool ValidateBook()
        {
            List<string> errors = new List<string>();

            string itemErrors = _itemService.ValidateItemProperties(Book);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

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

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; 
        }

    }
}
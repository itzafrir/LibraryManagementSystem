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
    public class EBookViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        public EBook EBook { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EBookViewModel(EBook ebook, ItemService itemService, Action closeAction)
        {
            EBook = ebook ?? new EBook();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

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

        private bool ValidateEBook()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(EBook);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific EBook properties
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

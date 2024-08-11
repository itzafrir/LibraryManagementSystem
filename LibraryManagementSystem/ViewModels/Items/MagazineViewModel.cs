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
    public class MagazineViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        public Magazine Magazine{ get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public MagazineViewModel(Magazine magazine, ItemService itemService, Action closeAction)
        {
            Magazine = magazine ?? new Magazine();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

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
        private bool ValidateMagazine()
        {
            List<string> errors = new List<string>();

            // Validate common item properties
            string itemErrors = _itemService.ValidateItemProperties(Magazine);
            if (!string.IsNullOrEmpty(itemErrors))
            {
                errors.Add(itemErrors);
            }

            // Validate specific Magazine properties
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

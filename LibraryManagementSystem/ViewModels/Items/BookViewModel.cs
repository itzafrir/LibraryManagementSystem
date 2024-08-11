using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
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
            // Example of trying to set total copies
            bool success = Book.TrySetTotalCopies(Book.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            if (Book.Id == 0)
            {
                Book.AvailableCopies = Book.TotalCopies;
                _itemService.AddItem(Book);
            }
            else
            {
                _itemService.UpdateItem(Book);
            }

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}
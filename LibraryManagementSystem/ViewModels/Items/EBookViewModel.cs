using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
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
            bool success = EBook.TrySetTotalCopies(EBook.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            if (EBook.Id == 0)
            {
                EBook.AvailableCopies = EBook.TotalCopies;
                _itemService.AddItem(EBook);
            }
            else
            {
                _itemService.UpdateItem(EBook);
            }

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}

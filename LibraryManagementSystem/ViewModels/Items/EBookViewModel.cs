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
        private EBook _eBook;

        public EBook EBook
        {
            get => _eBook;
            set => SetProperty(ref _eBook, value);
        }

        public EBookViewModel(EBook eBook, ItemService itemService)
        {
            _itemService = itemService;
            _eBook = eBook;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            if (IsValid())
            {
                if (_eBook.Id == 0)
                {
                    _itemService.AddItem(_eBook);
                }
                else
                {
                    _itemService.UpdateItem(_eBook);
                }
                CloseAction();
            }
            else
            {
                ShowValidationError();
            }
        }

        private void Cancel()
        {
            CloseAction();
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_eBook.Title) &&
                   !string.IsNullOrWhiteSpace(_eBook.Author) &&
                   !string.IsNullOrWhiteSpace(_eBook.FileFormat) &&
                   _eBook.FileSize > 0 &&
                   !string.IsNullOrWhiteSpace(_eBook.DownloadLink) &&
                   _eBook.TotalCopies > 0 &&
                   _eBook.AvailableCopies >= 0 &&
                   _eBook.TotalCopies >= _eBook.AvailableCopies;
        }

        private void ShowValidationError()
        {
            MessageBox.Show("Please fill in all fields and ensure that Total Copies is greater than or equal to Available Copies.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public Action CloseAction { get; set; }
    }
}

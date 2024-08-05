using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class MagazineViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private Magazine _magazine;

        public Magazine Magazine
        {
            get => _magazine;
            set => SetProperty(ref _magazine, value);
        }

        public MagazineViewModel(Magazine magazine, ItemService itemService)
        {
            _itemService = itemService;
            _magazine = magazine;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            if (IsValid())
            {
                if (_magazine.Id == 0)
                {
                    _itemService.AddItem(_magazine);
                }
                else
                {
                    _itemService.UpdateItem(_magazine);
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
            return !string.IsNullOrWhiteSpace(_magazine.Title) &&
                   !string.IsNullOrWhiteSpace(_magazine.Editor) &&
                   _magazine.IssueNumber > 0 &&
                   !string.IsNullOrWhiteSpace(_magazine.Genre) &&
                   !string.IsNullOrWhiteSpace(_magazine.Frequency) &&
                   _magazine.TotalCopies > 0 &&
                   _magazine.AvailableCopies >= 0 &&
                   _magazine.TotalCopies >= _magazine.AvailableCopies;
        }

        private void ShowValidationError()
        {
            MessageBox.Show("Please fill in all fields and ensure that Total Copies is greater than or equal to Available Copies.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public Action CloseAction { get; set; }
    }
}

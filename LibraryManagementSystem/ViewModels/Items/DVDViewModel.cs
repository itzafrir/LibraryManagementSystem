using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class DVDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private DVD _dvd;

        public DVD DVD
        {
            get => _dvd;
            set => SetProperty(ref _dvd, value);
        }

        public DVDViewModel(DVD dvd, ItemService itemService)
        {
            _itemService = itemService;
            _dvd = dvd;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save()
        {
            if (IsValid())
            {
                if (_dvd.Id == 0)
                {
                    _itemService.AddItem(_dvd);
                }
                else
                {
                    _itemService.UpdateItem(_dvd);
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
            return !string.IsNullOrWhiteSpace(_dvd.Title) &&
                   !string.IsNullOrWhiteSpace(_dvd.Director) &&
                   !string.IsNullOrWhiteSpace(_dvd.Genre) &&
                   _dvd.Duration != default &&
                   !string.IsNullOrWhiteSpace(_dvd.Language) &&
                   !string.IsNullOrWhiteSpace(_dvd.Studio) &&
                   _dvd.ReleaseDate != default &&
                   _dvd.TotalCopies > 0 &&
                   _dvd.AvailableCopies >= 0 &&
                   _dvd.TotalCopies >= _dvd.AvailableCopies;
        }

        private void ShowValidationError()
        {
            MessageBox.Show("Please fill in all fields and ensure that Total Copies is greater than or equal to Available Copies.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public Action CloseAction { get; set; }
    }
}

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
        private readonly Action _closeAction;

        public DVD DVD { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public DVDViewModel(DVD dvd, ItemService itemService, Action closeAction)
        {
            DVD = dvd ?? new DVD();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            bool success = DVD.TrySetTotalCopies(DVD.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            if (DVD.Id == 0)
            {
                DVD.AvailableCopies = DVD.TotalCopies;
                _itemService.AddItem(DVD);
            }
            else
            {
                _itemService.UpdateItem(DVD);
            }

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}

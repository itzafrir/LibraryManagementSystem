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
            bool success = Magazine.TrySetTotalCopies(Magazine.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            if (Magazine.Id == 0)
            {
                Magazine.AvailableCopies = Magazine.TotalCopies;
                _itemService.AddItem(Magazine);
            }
            else
            {
                _itemService.UpdateItem(Magazine);
            }

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}

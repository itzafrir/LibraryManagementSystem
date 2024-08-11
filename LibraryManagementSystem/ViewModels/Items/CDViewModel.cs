using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class CDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        public CD CD { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CDViewModel(CD cd, ItemService itemService, Action closeAction)
        {
            CD = cd ?? new CD();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            bool success = CD.TrySetTotalCopies(CD.TotalCopies);

            if (!success)
            {
                MessageBox.Show("Cannot reduce total copies because it would result in negative available copies.", "Invalid Operation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Prevent saving if the operation is invalid
            }

            if (CD.Id == 0)
            {
                CD.AvailableCopies = CD.TotalCopies;
                _itemService.AddItem(CD);
            }
            else
            {
                _itemService.UpdateItem(CD);
            }

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}

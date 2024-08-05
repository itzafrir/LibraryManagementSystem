using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class CDViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeWindow;

        public CD CD { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CDViewModel(CD cd, ItemService itemService, Action closeWindow)
        {
            CD = cd ?? new CD();
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _closeWindow = closeWindow ?? throw new ArgumentNullException(nameof(closeWindow));

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (CD.Id == 0)
            {
                _itemService.AddItem(CD);
            }
            else
            {
                _itemService.UpdateItem(CD);
            }
            _closeWindow();
        }

        private void Cancel()
        {
            _closeWindow();
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(CD.Title) &&
                   !string.IsNullOrWhiteSpace(CD.Artist);
        }
    }
}

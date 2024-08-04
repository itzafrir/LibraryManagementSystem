using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows;
using System.Windows.Input;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.ViewModels
{
    public partial class AddUpdateItemWindowViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeAction;

        public string WindowTitle { get; private set; }
        public ItemType SelectedItemType { get; set; }
        public string ItemTitle { get; set; }
        public string ItemDescription { get; set; }
        public string ItemISBN { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddUpdateItemWindowViewModel(Item item, ItemService itemService, Action closeAction)
        {
            _itemService = itemService;
            _closeAction = closeAction;

            if (item != null)
            {
                WindowTitle = "Update Item";
                SelectedItemType = item.ItemType;
                ItemTitle = item.Title;
                ItemDescription = item.Description;
                ItemISBN = item.ISBN;
            }
            else
            {
                WindowTitle = "Add New Item";
            }

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(ItemTitle) || string.IsNullOrEmpty(ItemDescription) || string.IsNullOrEmpty(ItemISBN))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var item = new Item
            {
                Title = ItemTitle,
                Description = ItemDescription,
                ISBN = ItemISBN,
                ItemType = SelectedItemType
            };

            if (WindowTitle == "Update Item")
            {
                _itemService.UpdateItem(item);
                MessageBox.Show("Item updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _itemService.AddItem(item);
                MessageBox.Show("Item added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}

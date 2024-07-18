using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class ItemDetailViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        public Item SelectedItem { get; }

        public ICommand LoanItemCommand { get; }

        public ItemDetailViewModel(Item selectedItem, ItemService itemService)
        {
            SelectedItem = selectedItem;
            _itemService = itemService;

            LoanItemCommand = new RelayCommand(LoanItem);
        }

        private void LoanItem()
        {
            // Implement loan logic here
            MessageBox.Show("Item loaned successfully.", "Success");
        }
    }
}

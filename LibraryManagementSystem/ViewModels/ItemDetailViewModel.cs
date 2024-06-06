using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    public partial class ItemDetailViewModel : ObservableObject
    {
        public Item SelectedItem { get; }

        public IRelayCommand LoanItemCommand { get; }

        public ItemDetailViewModel(Item selectedItem)
        {
            SelectedItem = selectedItem;
            LoanItemCommand = new RelayCommand(LoanItem);
        }

        private void LoanItem()
        {
            // Logic to loan the item
            MessageBox.Show("Item loaned successfully.", "Success");
        }
    }
}
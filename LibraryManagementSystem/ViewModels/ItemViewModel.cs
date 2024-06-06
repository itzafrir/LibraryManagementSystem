using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Mementos;
using System.Collections.ObjectModel;
using System.Windows;
using LibraryManagementSystem.Utilities.Enums;
using System.Linq;

namespace LibraryManagementSystem.ViewModels
{
    public partial class ItemViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly ItemCaretaker _caretaker;
        private Item _selectedItem;
        private string _searchTerm;

        public ObservableCollection<Item> Books { get; private set; }
        public ObservableCollection<Item> CDs { get; private set; }
        public ObservableCollection<Item> EBooks { get; private set; }
        public ObservableCollection<Item> DVDs { get; private set; }
        public ObservableCollection<Item> Magazines { get; private set; }

        public Item SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
        }

        public IRelayCommand SearchCommand { get; private set; }
        public IRelayCommand LoanItemCommand { get; private set; }
        public IRelayCommand UndoCommand { get; private set; }

        public ItemViewModel(ItemService itemService)
        {
            _itemService = itemService;
            _caretaker = new ItemCaretaker();
            Books = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.Book));
            CDs = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.CD));
            EBooks = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.EBook));
            DVDs = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.DVD));
            Magazines = new ObservableCollection<Item>(_itemService.GetItemsByType(ItemType.Magazine));

            SearchCommand = new RelayCommand(SearchItems);
            LoanItemCommand = new RelayCommand(LoanItem, CanExecuteLoanItem);
            UndoCommand = new RelayCommand(Undo, CanExecuteUndo);
        }

        private void SearchItems()
        {
            Books.Clear();
            CDs.Clear();
            EBooks.Clear();
            DVDs.Clear();
            Magazines.Clear();

            var searchResults = _itemService.SearchItems(SearchTerm);
            foreach (var item in searchResults)
            {
                switch (item.ItemType)
                {
                    case ItemType.Book:
                        Books.Add(item);
                        break;
                    case ItemType.CD:
                        CDs.Add(item);
                        break;
                    case ItemType.EBook:
                        EBooks.Add(item);
                        break;
                    case ItemType.DVD:
                        DVDs.Add(item);
                        break;
                    case ItemType.Magazine:
                        Magazines.Add(item);
                        break;
                }
            }
        }

        private bool CanExecuteLoanItem() => SelectedItem != null;

        private void LoanItem()
        {
            if (SelectedItem != null)
            {
                // Assuming the user is provided through some context
                // User user = ...;

                // For now, let's use a dummy user for demonstration purposes
                User user = new User { Id = 1, Username = "demo_user" };

                _caretaker.SaveState(SelectedItem);
                _itemService.LoanItem(user, SelectedItem);
                MessageBox.Show("Item loaned successfully.", "Success");
            }
        }

        private bool CanExecuteUndo() => SelectedItem != null && _caretaker.CanUndo();

        private void Undo()
        {
            if (SelectedItem != null)
            {
                _caretaker.Undo(SelectedItem);
                OnPropertyChanged(nameof(SelectedItem));
                MessageBox.Show("Undo successful.", "Success");
            }
        }
    }
}

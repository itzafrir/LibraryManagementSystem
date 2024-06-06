using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Collections.ObjectModel;

namespace LibraryManagementSystem.ViewModels
{
    public partial class CatalogViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private Item _selectedItem;
        private string _searchTerm;

        public ObservableCollection<Item> Items { get; }

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

        public IRelayCommand SearchCommand { get; }
        public IRelayCommand ViewItemDetailsCommand { get; }

        public CatalogViewModel(ItemService itemService)
        {
            _itemService = itemService;
            Items = new ObservableCollection<Item>(_itemService.GetAllItems());

            SearchCommand = new RelayCommand(SearchItems);
            ViewItemDetailsCommand = new RelayCommand(ViewItemDetails);
        }

        private void SearchItems()
        {
            Items.Clear();
            var searchResults = _itemService.SearchItems(SearchTerm);
            foreach (var item in searchResults)
            {
                Items.Add(item);
            }
        }

        private void ViewItemDetails()
        {
            if (SelectedItem != null)
            {
                new Views.ItemDetailPage { DataContext = new ItemDetailViewModel(SelectedItem) }.Show();
            }
        }
    }
}
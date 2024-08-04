using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.ViewModels
{
    public partial class AddUpdateItemWindowViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _onSave;

        [ObservableProperty]
        private Item _item;

        [ObservableProperty]
        private ItemType _selectedItemType;

        public ObservableCollection<ItemType> ItemTypes { get; }

        public bool IsItemTypeSelectionEnabled => _item.Id == 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddUpdateItemWindowViewModel(ItemService itemService, Item item, Action onSave)
        {
            _itemService = itemService;
            _item = item ?? new Item();
            _selectedItemType = item?.ItemType ?? ItemType.Book; // Default to Book if adding a new item
            _onSave = onSave;

            ItemTypes = new ObservableCollection<ItemType>(Enum.GetValues(typeof(ItemType)).Cast<ItemType>());

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            UpdateItemTemplate();
        }

        private void Save()
        {
            _item.ItemType = _selectedItemType;

            if (_item.Id == 0)
            {
                _itemService.AddItem(_item);
            }
            else
            {
                _itemService.UpdateItem(_item);
            }
            _onSave?.Invoke();
            CloseWindow();
        }

        private void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            App.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
        }

        partial void OnSelectedItemTypeChanged(ItemType value)
        {
            UpdateItemTemplate();
        }

        private void UpdateItemTemplate()
        {
            switch (_selectedItemType)
            {
                case ItemType.Book:
                    if (_item is not Book) _item = new Book();
                    break;
                case ItemType.CD:
                    if (_item is not CD) _item = new CD();
                    break;
                case ItemType.EBook:
                    if (_item is not EBook) _item = new EBook();
                    break;
                case ItemType.DVD:
                    if (_item is not DVD) _item = new DVD();
                    break;
                case ItemType.Magazine:
                    if (_item is not Magazine) _item = new Magazine();
                    break;
                default:
                    _item = new Item();
                    break;
            }
            OnPropertyChanged(nameof(Item));
        }
    }
}

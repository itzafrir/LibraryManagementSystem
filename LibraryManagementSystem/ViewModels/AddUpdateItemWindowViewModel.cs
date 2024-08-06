//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using LibraryManagementSystem.Models;
//using LibraryManagementSystem.Services;
//using System;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Windows;
//using System.Windows.Input;
//using LibraryManagementSystem.Utilities.Enums;

//namespace LibraryManagementSystem.ViewModels
//{
//    public partial class AddUpdateItemWindowViewModel : ObservableObject
//    {
//        private readonly ItemService _itemService;
//        private readonly Action _onSave;

//        [ObservableProperty]
//        private Item _item;

//        [ObservableProperty]
//        private ItemType _selectedItemType;

//        public ObservableCollection<ItemType> ItemTypes { get; }

//        public bool IsItemTypeSelectionEnabled => _item.Id == 0;

//        public ICommand SaveCommand { get; }
//        public ICommand CancelCommand { get; }

//        public AddUpdateItemWindowViewModel(ItemService itemService, Item item, Action onSave)
//        {
//            _itemService = itemService;
//            _onSave = onSave;

//            ItemTypes = new ObservableCollection<ItemType>(Enum.GetValues(typeof(ItemType)).Cast<ItemType>());

//            if (item != null && item.Id > 0)
//            {
//                // Fetch the item from the database
//                _item = _itemService.GetItemById(item.Id);
//                _selectedItemType = _item.ItemType;
//            }
//            else
//            {
//                _item = new Item();
//                _selectedItemType = ItemType.Book;
//                UpdateItemTemplate();
//            }

//            SaveCommand = new RelayCommand(Save);
//            CancelCommand = new RelayCommand(Cancel);
//        }

//        private void Save()
//        {
//            _item.ItemType = _selectedItemType;

//            if (_item.Id == 0)
//            {
//                _itemService.AddItem(_item);
//            }
//            else
//            {
//                _itemService.UpdateItem(_item);
//            }
//            _onSave?.Invoke();
//            CloseWindow();
//        }

//        private void Cancel()
//        {
//            CloseWindow();
//        }

//        private void CloseWindow()
//        {
//            App.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
//        }

//        partial void OnSelectedItemTypeChanged(ItemType value)
//        {
//            UpdateItemTemplate();
//        }

//        private void UpdateItemTemplate()
//        {
//            Item newItem;
//            switch (_selectedItemType)
//            {
//                case ItemType.Book:
//                    newItem = _item is Book book ? book : new Book { Id = _item.Id, Title = _item.Title };
//                    break;
//                case ItemType.CD:
//                    newItem = _item is CD cd ? cd : new CD { Id = _item.Id, Title = _item.Title };
//                    break;
//                case ItemType.EBook:
//                    newItem = _item is EBook eBook ? eBook : new EBook { Id = _item.Id, Title = _item.Title };
//                    break;
//                case ItemType.DVD:
//                    newItem = _item is DVD dvd ? dvd : new DVD { Id = _item.Id, Title = _item.Title };
//                    break;
//                case ItemType.Magazine:
//                    newItem = _item is Magazine magazine ? magazine : new Magazine { Id = _item.Id, Title = _item.Title };
//                    break;
//                default:
//                    newItem = new Item { Id = _item.Id, Title = _item.Title };
//                    break;
//            }

//            _item = newItem;
//            OnPropertyChanged(nameof(Item));
//        }
//    }
//}

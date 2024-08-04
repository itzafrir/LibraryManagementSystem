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
                    if (_item is not Book)
                    {
                        var book = _item as Book ?? new Book();
                        _item = new Book
                        {
                            Title = _item.Title,
                            Author = book.Author,
                            Genre = book.Genre,
                            PageCount = book.PageCount,
                            Language = book.Language,
                            Format = book.Format,
                            Dimensions = book.Dimensions,
                            Series = book.Series,
                            Edition = book.Edition,
                            Keywords = book.Keywords
                        };
                    }
                    break;
                case ItemType.CD:
                    if (_item is not CD)
                    {
                        var cd = _item as CD ?? new CD();
                        _item = new CD
                        {
                            Title = _item.Title,
                            Artist = cd.Artist,
                            Genre = cd.Genre,
                            Duration = cd.Duration,
                            TrackCount = cd.TrackCount,
                            Label = cd.Label,
                            ReleaseDate = cd.ReleaseDate,
                            Tracks = cd.Tracks
                        };
                    }
                    break;
                case ItemType.EBook:
                    if (_item is not EBook)
                    {
                        var ebook = _item as EBook ?? new EBook();
                        _item = new EBook
                        {
                            Title = _item.Title,
                            Author = ebook.Author,
                            FileFormat = ebook.FileFormat,
                            FileSize = ebook.FileSize,
                            DownloadLink = ebook.DownloadLink,
                            Keywords = ebook.Keywords
                        };
                    }
                    break;
                case ItemType.DVD:
                    if (_item is not DVD)
                    {
                        var dvd = _item as DVD ?? new DVD();
                        _item = new DVD
                        {
                            Title = _item.Title,
                            Director = dvd.Director,
                            Genre = dvd.Genre,
                            Duration = dvd.Duration,
                            Language = dvd.Language,
                            Studio = dvd.Studio,
                            ReleaseDate = dvd.ReleaseDate,
                            Subtitles = dvd.Subtitles,
                            Cast = dvd.Cast
                        };
                    }
                    break;
                case ItemType.Magazine:
                    if (_item is not Magazine)
                    {
                        var magazine = _item as Magazine ?? new Magazine();
                        _item = new Magazine
                        {
                            Title = _item.Title,
                            Editor = magazine.Editor,
                            IssueNumber = magazine.IssueNumber,
                            Genre = magazine.Genre,
                            Frequency = magazine.Frequency,
                            Articles = magazine.Articles
                        };
                    }
                    break;
                default:
                    _item = new Item();
                    break;
            }
            OnPropertyChanged(nameof(Item));
        }
    }
}

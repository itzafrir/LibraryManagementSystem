using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class BookViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly Action _closeWindow;

        public Book Book { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public BookViewModel(Book book, ItemService itemService, Action closeWindow)
        {
            Book = book ?? new Book();
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _closeWindow = closeWindow ?? throw new ArgumentNullException(nameof(closeWindow));

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (Book.Id == 0)
            {
                _itemService.AddItem(Book);
            }
            else
            {
                _itemService.UpdateItem(Book);
            }
            _closeWindow();
        }

        private void Cancel()
        {
            _closeWindow();
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Book.Title) &&
                   !string.IsNullOrWhiteSpace(Book.Author);
        }
    }
}
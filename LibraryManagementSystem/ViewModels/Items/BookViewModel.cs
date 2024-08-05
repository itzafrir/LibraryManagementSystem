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
        private readonly Action _closeAction;

        public Book Book { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public BookViewModel(Book book, ItemService itemService, Action closeAction)
        {
            Book = book ?? new Book();
            _itemService = itemService;
            _closeAction = closeAction;

            SaveCommand = new RelayCommand(Save);
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

            _closeAction();
        }

        private void Cancel()
        {
            _closeAction();
        }
    }
}
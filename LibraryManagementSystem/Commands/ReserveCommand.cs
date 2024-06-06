using System;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Windows;

namespace LibraryManagementSystem.Commands
{
    public class ReserveCommand : IRelayCommand
    {
        private readonly ItemService _itemService;
        private readonly User _user;
        private readonly Item _item;

        public ReserveCommand(ItemService itemService, User user, Item item)
        {
            _itemService = itemService;
            _user = user;
            _item = item;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _item != null && _user != null;

        public void Execute(object parameter)
        {
            _itemService.ReserveItem(_user, _item);
            MessageBox.Show("Item reserved successfully.", "Success");
            NotifyCanExecuteChanged();
        }

        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
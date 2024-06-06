using System;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Windows;

namespace LibraryManagementSystem.Commands
{
    public class ReturnCommand : IRelayCommand
    {
        private readonly ItemService _itemService;
        private readonly Loan _loan;

        public ReturnCommand(ItemService itemService, Loan loan)
        {
            _itemService = itemService;
            _loan = loan;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _loan != null;

        public void Execute(object parameter)
        {
            _itemService.ReturnItem(_loan);
            MessageBox.Show("Item returned successfully.", "Success");
            NotifyCanExecuteChanged();
        }

        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
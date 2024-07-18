using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Utilities;
using System.Windows.Input;
using System;

namespace LibraryManagementSystem.ViewModels
{
    public class ItemDetailViewModel : ObservableObject
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly CatalogViewModel _catalogViewModel;
        private readonly CatalogViewModelMemento _catalogViewModelMemento;
        public Item SelectedItem { get; }

        public ICommand LoanItemCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand GoBackToCatalogCommand { get; }

        public event EventHandler RequestClose;

        public ItemDetailViewModel(Item selectedItem, ItemService itemService, UserService userService, CatalogViewModel catalogViewModel)
        {
            SelectedItem = selectedItem;
            _itemService = itemService;
            _userService = userService;
            _catalogViewModel = catalogViewModel;
            _catalogViewModelMemento = _catalogViewModel.CreateMemento();

            LoanItemCommand = new RelayCommand(LoanItem);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            GoBackToCatalogCommand = new RelayCommand(GoBackToCatalog);
        }

        private void LoanItem()
        {
            // Implement loan logic here
            MessageBox.Show("Item loaned successfully.", "Success");
        }

        private void NavigateHome()
        {
            var mainWindow = new Views.MainWindow(_userService, _itemService);
            mainWindow.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private void GoBackToCatalog()
        {
            var catalogPage = new Views.CatalogPage(_itemService, _userService)
            {
                DataContext = _catalogViewModel
            };
            _catalogViewModel.RestoreMemento(_catalogViewModelMemento);
            catalogPage.Show();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}

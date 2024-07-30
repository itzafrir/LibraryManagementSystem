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
        public Item SelectedItem { get;private set; }

        public ICommand LoanItemCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand GoBackToCatalogCommand { get; }
        public IRelayCommand NavigateToLoginCommand { get; }

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
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin); // Add this line
        }
        public ItemDetailViewModelMemento CreateMemento()
        {
            return new ItemDetailViewModelMemento(SelectedItem);
        }

        public void RestoreMemento(ItemDetailViewModelMemento memento)
        {
            SelectedItem = memento.SelectedItem;
            // You may need to notify property changes if necessary
            OnPropertyChanged(nameof(SelectedItem));
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
            RequestClose?.Invoke(this, EventArgs.Empty); // Invoke the event to close the window
        }

        private void GoBackToCatalog()
        {
            var catalogPage = new Views.CatalogPage(_itemService, _userService)
            {
                DataContext = _catalogViewModel
            };
            // Subscribe to the RequestClose event here
            ((CatalogViewModel)catalogPage.DataContext).RequestClose += (_, __) => catalogPage.Close();
            _catalogViewModel.RestoreMemento(_catalogViewModelMemento);
            catalogPage.Show();
            RequestClose?.Invoke(this, EventArgs.Empty); // Ensure this is invoked
        }
        private void NavigateToLogin()
        {
            var memento = CreateMemento();
            var loginWindow = new Views.LoginWindow(_userService, () => GoBackToItemDetail(memento), NavigateHome)
            {
                DataContext = new LoginViewModel(_userService, () => GoBackToItemDetail(memento), NavigateHome)
            };
            loginWindow.Show();
            ((LoginViewModel)loginWindow.DataContext).RequestClose += (_, __) => loginWindow.Close();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        private void GoBackToItemDetail(ItemDetailViewModelMemento memento)
        {
            var itemDetailPage = new Views.ItemDetailPage(memento.SelectedItem, _itemService, _userService, _catalogViewModel)
            {
                DataContext = this
            };
            ((ItemDetailViewModel)itemDetailPage.DataContext).RequestClose += (_, __) => itemDetailPage.Close();
            RestoreMemento(memento);
            itemDetailPage.Show();
        }
    }
}

using System;
using System.Windows;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class ItemDetailPage : Window
    {
        public ItemDetailPage(Item selectedItem, ItemService itemService, UserService userService, CatalogViewModel catalogViewModel)
        {
            InitializeComponent();
            var viewModel = new ItemDetailViewModel(selectedItem, itemService, userService, catalogViewModel);
            viewModel.RequestClose += OnRequestClose;
            DataContext = viewModel;
        }

        private void OnRequestClose(object sender, EventArgs e)
        {
            Close(); // Ensure this is called
        }
    }
}
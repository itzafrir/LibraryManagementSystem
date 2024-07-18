using System;
using System.Windows;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class CatalogPage : Window
    {
        public CatalogPage(ItemService itemService, UserService userService)
        {
            InitializeComponent();
            var viewModel = new CatalogViewModel(itemService, userService);
            viewModel.RequestClose += OnRequestClose;
            DataContext = viewModel;
        }

        private void OnRequestClose(object sender, EventArgs e)
        {
            Close();
        }
    }
}
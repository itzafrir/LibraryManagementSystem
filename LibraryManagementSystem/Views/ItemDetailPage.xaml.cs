using System;
using System.Windows;
using System.Windows.Controls;
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
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                ((ItemDetailViewModel)DataContext).NewReviewRating = int.Parse(comboBox.SelectedValue.ToString());
            }
        }
    }
}
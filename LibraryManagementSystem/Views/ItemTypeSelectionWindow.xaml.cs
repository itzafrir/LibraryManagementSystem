using System;
using System.Collections.Generic;
using System.Windows;
using LibraryManagementSystem.Utilities.Enums;

namespace LibraryManagementSystem.Views
{
    public partial class ItemTypeSelectionWindow : Window
    {
        public ItemType? SelectedItemType { get; private set; }

        public ItemTypeSelectionWindow()
        {
            InitializeComponent();
            ItemTypeComboBox.ItemsSource = Enum.GetValues(typeof(ItemType));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemTypeComboBox.SelectedValue is ItemType selectedItemType)
            {
                SelectedItemType = selectedItemType;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please select an item type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}